using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Interfaces;

/// <summary>
/// Defines persistence operations for student records.
/// </summary>
public interface IStudentRepository
{
	/// <summary>
	/// Adds a student if no record already uses the student's ID.
	/// </summary>
	/// <param name="student">The student to add.</param>
	/// <returns><see langword="true"/> if added; otherwise, <see langword="false"/> if the ID already exists.</returns>
	bool AddStudent(Student student);

	/// <summary>
	/// Retrieves all stored students.
	/// </summary>
	/// <returns>A read-only list of students.</returns>
	IReadOnlyList<Student> GetAllStudents();

	/// <summary>
	/// Finds a stored student by ID.
	/// </summary>
	/// <param name="studentId">The ID of the student to find.</param>
	/// <returns>The matching student, or <see langword="null"/> if no student is found.</returns>
	Student? GetStudentById(int studentId);

	/// <summary>
	/// Updates an existing student record.
	/// </summary>
	/// <param name="student">The student details to save.</param>
	/// <returns><see langword="true"/> if updated; otherwise, <see langword="false"/> if the student does not exist.</returns>
	bool UpdateStudent(Student student);

	/// <summary>
	/// Deletes a stored student by ID.
	/// </summary>
	/// <param name="studentId">The ID of the student to delete.</param>
	/// <returns><see langword="true"/> if deleted; otherwise, <see langword="false"/> if no student has that ID.</returns>
	bool DeleteStudent(int studentId);
}
