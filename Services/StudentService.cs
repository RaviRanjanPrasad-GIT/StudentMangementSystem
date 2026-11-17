using System.ComponentModel.DataAnnotations;
using StudentManagement.ConsoleApp.Interfaces;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Services;

/// <summary>
/// Provides business operations for managing students.
/// </summary>
public class StudentService : IStudentService
{
	private readonly IStudentRepository _studentRepository;

	/// <summary>
	/// Creates the service with the repository used to access student records.
	/// </summary>
	/// <param name="studentRepository">The student repository.</param>
	/// <exception cref="ArgumentNullException"><paramref name="studentRepository"/> is <see langword="null"/>.</exception>
	public StudentService(IStudentRepository studentRepository)
	{
		ArgumentNullException.ThrowIfNull(studentRepository);
		_studentRepository = studentRepository;
	}

	/// <inheritdoc />
	public bool AddStudent(Student student)
	{
		ValidateStudent(student);
		return _studentRepository.AddStudent(student);
	}

	/// <inheritdoc />
	public IReadOnlyList<Student> GetAllStudents()
	{
		return _studentRepository.GetAllStudents();
	}

	/// <inheritdoc />
	public Student? GetStudentById(int studentId)
	{
		return _studentRepository.GetStudentById(studentId);
	}

	/// <inheritdoc />
	public bool UpdateStudent(Student student)
	{
		ValidateStudent(student);
		return _studentRepository.UpdateStudent(student);
	}

	/// <inheritdoc />
	public bool DeleteStudent(int studentId)
	{
		return _studentRepository.DeleteStudent(studentId);
	}

	private static void ValidateStudent(Student student)
	{
		ArgumentNullException.ThrowIfNull(student);

		var validationContext = new ValidationContext(student);
		Validator.ValidateObject(student, validationContext, validateAllProperties: true);
	}
}
