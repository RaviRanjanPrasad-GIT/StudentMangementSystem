using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Interfaces;

/// <summary>
/// Defines student-management operations provided by the business layer.
/// </summary>
public interface IStudentService
{
	/// <summary>
	/// Adds a student when the student ID is not already in use.
	/// </summary>
	/// <param name="student">The student to add.</param>
	/// <returns><see langword="true"/> if added; otherwise, <see langword="false"/> if the ID already exists.</returns>
	bool AddStudent(Student student);

	/// <summary>
	/// Gets all students.
	/// </summary>
	/// <returns>A read-only list of students.</returns>
	IReadOnlyList<Student> GetAllStudents();

	/// <summary>
	/// Finds a student by ID.
	/// </summary>
	/// <param name="studentId">The ID of the student to find.</param>
	/// <returns>The matching student, or <see langword="null"/> if no student is found.</returns>
	Student? GetStudentById(int studentId);

	/// <summary>
	/// Updates an existing student without changing the student's ID.
	/// </summary>
	/// <param name="student">The student details to save.</param>
	/// <returns><see langword="true"/> if updated; otherwise, <see langword="false"/> if the student does not exist.</returns>
	bool UpdateStudent(Student student);

	/// <summary>
	/// Deletes a student by ID.
	/// </summary>
	/// <param name="studentId">The ID of the student to delete.</param>
	/// <returns><see langword="true"/> if deleted; otherwise, <see langword="false"/> if no student has that ID.</returns>
	bool DeleteStudent(int studentId);
}
