using System.ComponentModel.DataAnnotations;
using StudentManagement.ConsoleApp.Interfaces;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Presentations;

/// <summary>
/// Provides the interactive console menu for student-management operations.
/// </summary>
public class ConsoleMenu
{
	private readonly IStudentService _studentService;

	/// <summary>
	/// Creates the menu with the student service used to perform operations.
	/// </summary>
	/// <param name="studentService">The student-management service.</param>
	public ConsoleMenu(IStudentService studentService)
	{
		ArgumentNullException.ThrowIfNull(studentService);
		_studentService = studentService;
	}

	/// <summary>
	/// Displays the menu until the operator chooses to exit.
	/// </summary>
	public void Run()
	{
		while (true)
		{
			DisplayMenu();
			Console.Write("Select an option: ");
			var choice = Console.ReadLine();

			if (choice is null)
			{
				Console.WriteLine("\nInput ended. Exiting the application.");
				return;
			}

			if (choice.Trim() == "0")
			{
				Console.WriteLine("Goodbye.");
				return;
			}

			try
			{
				switch (choice.Trim())
				{
					case "1":
						AddStudent();
						break;
					case "2":
						DisplayAllStudents();
						break;
					case "3":
						SearchStudent();
						break;
					case "4":
						UpdateStudent();
						break;
					case "5":
						DeleteStudent();
						break;
					default:
						Console.WriteLine("Please select an option from 0 to 5.");
						break;
				}
			}
			catch (EndOfStreamException)
			{
				Console.WriteLine("\nInput ended. Exiting the application.");
				return;
			}
			catch (ValidationException exception)
			{
				Console.WriteLine($"Student information is invalid: {exception.Message}");
			}
			catch (Exception exception)
			{
				Console.WriteLine($"The operation could not be completed: {exception.Message}");
			}

			Console.WriteLine();
		}
	}

	private static void DisplayMenu()
	{
		Console.WriteLine("Student Management System");
		Console.WriteLine("1. Add a student");
		Console.WriteLine("2. Display all students");
		Console.WriteLine("3. Search for a student");
		Console.WriteLine("4. Update a student");
		Console.WriteLine("5. Delete a student");
		Console.WriteLine("0. Exit");
	}

	private void AddStudent()
	{
		Console.WriteLine("\nAdd Student");
		var student = new Student
		{
			StudentId = ReadInteger("Student ID", 1, int.MaxValue),
			FirstName = ReadRequiredText("First name", 50),
			LastName = ReadRequiredText("Last name", 50),
			Email = ReadEmail("Email"),
			Age = ReadInteger("Age", 3, 100),
			Course = ReadRequiredText("Course", 100)
		};

		Console.WriteLine(_studentService.AddStudent(student)
			? "Student registered successfully."
			: $"Student ID {student.StudentId} already exists; no student was added.");
	}

	private void DisplayAllStudents()
	{
		var students = _studentService.GetAllStudents();
		if (students.Count == 0)
		{
			Console.WriteLine("\nThere are no students to display.");
			return;
		}

		Console.WriteLine("\nStudent Records");
		foreach (var student in students)
		{
			DisplayStudent(student);
		}
	}

	private void SearchStudent()
	{
		var studentId = ReadInteger("Student ID to search for", 1, int.MaxValue);
		var student = _studentService.GetStudentById(studentId);

		if (student is null)
		{
			Console.WriteLine($"No student was found with ID {studentId}.");
			return;
		}

		DisplayStudent(student);
	}

	private void UpdateStudent()
	{
		var studentId = ReadInteger("Student ID to update", 1, int.MaxValue);
		var student = _studentService.GetStudentById(studentId);
		if (student is null)
		{
			Console.WriteLine($"No student was found with ID {studentId}.");
			return;
		}

		Console.WriteLine("Leave a field blank to keep its current value.");
		student.FirstName = ReadUpdatedText("First name", student.FirstName, 50);
		student.LastName = ReadUpdatedText("Last name", student.LastName, 50);
		student.Email = ReadUpdatedEmail(student.Email);
		student.Age = ReadUpdatedInteger("Age", student.Age, 3, 100);
		student.Course = ReadUpdatedText("Course", student.Course, 100);

		Console.WriteLine(_studentService.UpdateStudent(student)
			? "Student details updated successfully."
			: $"Student ID {studentId} could not be updated because it no longer exists.");
	}

	private void DeleteStudent()
	{
		var studentId = ReadInteger("Student ID to delete", 1, int.MaxValue);
		var student = _studentService.GetStudentById(studentId);
		if (student is null)
		{
			Console.WriteLine($"No student was found with ID {studentId}.");
			return;
		}

		DisplayStudent(student);
		Console.Write("Delete this student? (y/N): ");
		var confirmation = ReadInput();
		if (!string.Equals(confirmation.Trim(), "y", StringComparison.OrdinalIgnoreCase))
		{
			Console.WriteLine("Deletion cancelled.");
			return;
		}

		Console.WriteLine(_studentService.DeleteStudent(studentId)
			? "Student deleted successfully."
			: $"Student ID {studentId} could not be deleted because it no longer exists.");
	}

	private static void DisplayStudent(Student student)
	{
		Console.WriteLine(
			$"ID: {student.StudentId} | Name: {student.FirstName} {student.LastName} | " +
			$"Email: {student.Email} | Age: {student.Age} | Course: {student.Course}");
	}

	private static string ReadRequiredText(string fieldName, int maximumLength)
	{
		while (true)
		{
			Console.Write($"{fieldName}: ");
			var value = ReadInput().Trim();
			if (value.Length == 0)
			{
				Console.WriteLine($"{fieldName} is required.");
				continue;
			}

			if (value.Length > maximumLength)
			{
				Console.WriteLine($"{fieldName} cannot exceed {maximumLength} characters.");
				continue;
			}

			return value;
		}
	}

	private static string ReadUpdatedText(string fieldName, string currentValue, int maximumLength)
	{
		while (true)
		{
			Console.Write($"{fieldName} ({currentValue}): ");
			var value = ReadInput().Trim();
			if (value.Length == 0)
			{
				return currentValue;
			}

			if (value.Length > maximumLength)
			{
				Console.WriteLine($"{fieldName} cannot exceed {maximumLength} characters.");
				continue;
			}

			return value;
		}
	}

	private static string ReadEmail(string fieldName)
	{
		while (true)
		{
			var email = ReadRequiredText(fieldName, 254);
			if (new EmailAddressAttribute().IsValid(email))
			{
				return email;
			}

			Console.WriteLine("Enter a valid email address.");
		}
	}

	private static string ReadUpdatedEmail(string currentEmail)
	{
		while (true)
		{
			Console.Write($"Email ({currentEmail}): ");
			var email = ReadInput().Trim();
			if (email.Length == 0)
			{
				return currentEmail;
			}

			if (email.Length <= 254 && new EmailAddressAttribute().IsValid(email))
			{
				return email;
			}

			Console.WriteLine("Enter a valid email address no longer than 254 characters.");
		}
	}

	private static int ReadInteger(string fieldName, int minimum, int maximum)
	{
		while (true)
		{
			Console.Write($"{fieldName}: ");
			if (int.TryParse(ReadInput(), out var value) && value >= minimum && value <= maximum)
			{
				return value;
			}

			Console.WriteLine($"Enter a whole number from {minimum} to {maximum}.");
		}
	}

	private static int ReadUpdatedInteger(string fieldName, int currentValue, int minimum, int maximum)
	{
		while (true)
		{
			Console.Write($"{fieldName} ({currentValue}): ");
			var input = ReadInput().Trim();
			if (input.Length == 0)
			{
				return currentValue;
			}

			if (int.TryParse(input, out var value) && value >= minimum && value <= maximum)
			{
				return value;
			}

			Console.WriteLine($"Enter a whole number from {minimum} to {maximum}, or leave blank to keep the current value.");
		}
	}

	private static string ReadInput()
	{
		return Console.ReadLine() ?? throw new EndOfStreamException();
	}
}
