using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using StudentManagement.ConsoleApp.Interfaces;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Repositories;

/// <summary>
/// Persists student records in a JSON file.
/// </summary>
public class StudentRepository : IStudentRepository
{
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		WriteIndented = true,
		PropertyNameCaseInsensitive = true
	};

	private readonly string _filePath;
	private List<Student> _students;

	/// <summary>
	/// Creates a repository using the specified file or Data/students.json in the current directory.
	/// </summary>
	/// <param name="filePath">Optional JSON file path, useful for tests or custom deployments.</param>
	public StudentRepository(string? filePath = null)
	{
		_filePath = string.IsNullOrWhiteSpace(filePath)
			? GetDefaultFilePath()
			: Path.GetFullPath(filePath);

		Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
		if (!File.Exists(_filePath))
		{
			SaveStudents([]);
		}

		_students = LoadStudents();
	}

	/// <inheritdoc />
	public bool AddStudent(Student student)
	{
		ArgumentNullException.ThrowIfNull(student);

		if (_students.Any(existing => existing.StudentId == student.StudentId))
		{
			return false;
		}

		var updatedStudents = _students.Select(CloneStudent).ToList();
		updatedStudents.Add(CloneStudent(student));
		SaveStudents(updatedStudents);
		_students = updatedStudents;
		return true;
	}

	/// <inheritdoc />
	public IReadOnlyList<Student> GetAllStudents()
	{
		return _students
			.OrderBy(student => student.StudentId)
			.Select(CloneStudent)
			.ToList()
			.AsReadOnly();
	}

	/// <inheritdoc />
	public Student? GetStudentById(int studentId)
	{
		var student = _students.FirstOrDefault(existing => existing.StudentId == studentId);
		return student is null ? null : CloneStudent(student);
	}

	/// <inheritdoc />
	public bool UpdateStudent(Student student)
	{
		ArgumentNullException.ThrowIfNull(student);

		var studentIndex = _students.FindIndex(existing => existing.StudentId == student.StudentId);
		if (studentIndex < 0)
		{
			return false;
		}

		var updatedStudents = _students.Select(CloneStudent).ToList();
		updatedStudents[studentIndex] = CloneStudent(student);
		SaveStudents(updatedStudents);
		_students = updatedStudents;
		return true;
	}

	/// <inheritdoc />
	public bool DeleteStudent(int studentId)
	{
		var studentIndex = _students.FindIndex(existing => existing.StudentId == studentId);
		if (studentIndex < 0)
		{
			return false;
		}

		var updatedStudents = _students.Select(CloneStudent).ToList();
		updatedStudents.RemoveAt(studentIndex);
		SaveStudents(updatedStudents);
		_students = updatedStudents;
		return true;
	}

	private List<Student> LoadStudents()
	{
		var json = File.ReadAllText(_filePath);
		if (string.IsNullOrWhiteSpace(json))
		{
			return [];
		}

		try
		{
			var students = JsonSerializer.Deserialize<List<Student?>>(json, JsonOptions)
				?? throw new InvalidDataException("The student data file does not contain a student collection.");
			var studentIds = new HashSet<int>();
			var validatedStudents = new List<Student>(students.Count);

			foreach (var student in students)
			{
				if (student is null)
				{
					throw new InvalidDataException("The student data file contains a null student record.");
				}

				try
				{
					Validator.ValidateObject(student, new ValidationContext(student), validateAllProperties: true);
				}
				catch (ValidationException exception)
				{
					throw new InvalidDataException(
						$"The student data file contains invalid data for student ID {student.StudentId}.",
						exception);
				}

				if (!studentIds.Add(student.StudentId))
				{
					throw new InvalidDataException(
						$"The student data file contains duplicate student ID {student.StudentId}.");
				}

				validatedStudents.Add(student);
			}

			return validatedStudents;
		}
		catch (JsonException exception)
		{
			throw new InvalidDataException(
				$"The student data file '{_filePath}' contains invalid JSON. The file was left unchanged.",
				exception);
		}
	}

	private void SaveStudents(List<Student> students)
	{
		var directory = Path.GetDirectoryName(_filePath)!;
		Directory.CreateDirectory(directory);

		var temporaryFilePath = Path.Combine(directory, $"{Path.GetFileName(_filePath)}.{Guid.NewGuid():N}.tmp");
		try
		{
			var json = JsonSerializer.Serialize(students, JsonOptions);
			File.WriteAllText(temporaryFilePath, json);
			File.Move(temporaryFilePath, _filePath, overwrite: true);
		}
		finally
		{
			TryDeleteTemporaryFile(temporaryFilePath);
		}
	}

	private static void TryDeleteTemporaryFile(string temporaryFilePath)
	{
		try
		{
			if (File.Exists(temporaryFilePath))
			{
				File.Delete(temporaryFilePath);
			}
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
	}

	private static Student CloneStudent(Student student)
	{
		return new Student
		{
			StudentId = student.StudentId,
			FirstName = student.FirstName,
			LastName = student.LastName,
			Email = student.Email,
			Age = student.Age,
			Course = student.Course
		};
	}

	private static string GetDefaultFilePath()
	{
		return Path.Combine(Directory.GetCurrentDirectory(), "Data", "students.json");
	}
}
