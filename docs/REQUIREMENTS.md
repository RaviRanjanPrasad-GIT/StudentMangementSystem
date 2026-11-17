# Student Management System Requirements

## 1. Purpose

This document defines the initial functional and non-functional requirements for the Student Management System, a .NET 10 console application. It is intended to guide implementation and provide a basis for verification. The requirements describe the target behavior; they do not imply that these capabilities are already implemented.

## 2. Scope

The initial release will allow an operator to create, list, find, update, and delete student records through a text-based console interface. Student records will be retained between application runs using persistent storage.

Course enrollment, grades, user accounts, role-based access, and reporting beyond listing students are outside the initial scope.

## 3. Actors

| Actor | Description |
|---|---|
| System operator | A person using the application locally to maintain student records. The initial release assumes one operator at a time and does not define authentication or concurrent access. |

## 4. Data Definitions and Business Rules

The initial student record contains the following fields:

| Field | Requirement |
|---|---|
| Student ID | Required, unique, and treated as the record's stable identifier. Leading and trailing whitespace is removed before validation and comparison. |
| First name | Required; must contain at least one non-whitespace character. |
| Last name | Required; must contain at least one non-whitespace character. |
| Date of birth | Required and must be a valid calendar date not later than the current date. |
| Email address | Optional; if provided, must be in a recognizable email-address format. |
| Phone number | Optional; if provided, must contain at least one digit. |

Student IDs are compared without regard to letter casing after trimming whitespace. A student ID must not be changed as part of the update workflow in the initial release. The application must preserve entered student names and optional contact details after trimming surrounding whitespace.

## 5. Functional Requirements

### FR-001: Add a New Student

The system shall provide an add-student workflow that collects the required student information and any optional contact information.

**Acceptance criteria:**

- The operator can enter a student ID, first name, last name, and date of birth.
- The operator may also enter an email address and phone number or leave either blank.
- The system validates the values before creating the record.
- The system confirms successful creation only after the record has been saved to persistent storage.
- If creation or saving fails, the system reports that the student was not saved and does not report success.

### FR-002: Display All Students

The system shall provide an option to display all student records currently held in persistent storage.

**Acceptance criteria:**

- Each displayed record includes the student ID, first name, last name, and date of birth.
- Email and phone are displayed when present, with absent optional values represented clearly.
- Records are presented in a consistent, readable layout and stable order (student ID ascending).
- If there are no records, the system displays a clear empty-state message rather than an empty or ambiguous screen.
- A storage read failure is reported without terminating the application unexpectedly.

### FR-003: Search for a Student by ID

The system shall allow the operator to search for one student using the student's ID.

**Acceptance criteria:**

- The search trims leading and trailing whitespace and uses the ID comparison rule defined in Section 4.
- When a match is found, the system displays all fields for that student.
- When no match is found, the system displays a clear not-found message and returns to a usable application state.
- A storage read failure is reported without presenting it as a not-found result.

### FR-004: Update Student Details

The system shall allow the operator to update an existing student's name, date of birth, email address, and phone number by supplying the student's ID.

**Acceptance criteria:**

- The system first confirms that the specified student exists.
- The student ID remains unchanged during the update.
- The system validates the complete resulting record before saving it.
- Invalid input leaves the previously saved record unchanged and allows the operator to correct the input or cancel.
- The system confirms success only after the update has been saved to persistent storage.
- If saving fails, the system reports the failure and retains the previously saved values.

### FR-005: Delete a Student

The system shall allow the operator to delete an existing student by student ID.

**Acceptance criteria:**

- The system identifies the target using the ID comparison rule defined in Section 4.
- The system displays the matched student's identifying details and requests confirmation before deletion.
- A cancellation leaves the record unchanged.
- A confirmed deletion is reported as successful only after it has been saved to persistent storage.
- If the ID does not match a record, the system reports that no student was deleted.
- If saving the deletion fails, the system reports the failure and retains the record.

### FR-006: Validate Student Information

The system shall validate student information when creating or updating a record, before any changes are committed.

**Acceptance criteria:**

- Student ID, first name, last name, and date of birth are required.
- Required text fields containing only whitespace are rejected.
- Date of birth must parse as a real calendar date and must not be in the future.
- A non-empty email address must have a recognizable email-address format.
- A non-empty phone number must contain at least one digit.
- Validation feedback identifies the field or fields that need correction and does not silently discard invalid data.
- Rejected input does not create, partially update, or otherwise corrupt a stored record.

### FR-007: Prevent Duplicate Student IDs

The system shall prevent creation of more than one student record with the same student ID.

**Acceptance criteria:**

- Duplicate checking occurs before a new record is committed.
- Duplicate checking follows the trimmed, case-insensitive comparison rule defined in Section 4.
- A duplicate ID is rejected with a clear message, and the existing record remains unchanged.
- An update to a student's non-ID fields does not treat that student's own ID as a duplicate.

### FR-008: Provide Menu Navigation

The system shall present a text-based menu for the supported student-management operations and allow the operator to return to the menu after completing or cancelling an operation.

**Acceptance criteria:**

- The menu identifies the available operations and the input expected to select one.
- An invalid menu selection produces a useful message and redisplays the menu.
- The operator can exit the application through an explicit menu choice.
- The application does not terminate solely because the operator enters an invalid menu choice or record value.

## 6. Non-Functional Requirements

### NFR-001: Maintainability

The implementation shall use consistent naming, formatting, and documented conventions so that a developer familiar with C# and .NET can understand and change the system.

**Verification:** A code review confirms that responsibilities and validation rules are understandable, duplicated business rules are avoided where practical, and public behavior is documented where it is not self-evident.

### NFR-002: Exception Handling

The system shall handle expected operational failures, including storage access and serialization failures, without an unhandled exception terminating the application during normal operation.

**Verification:** Inject or simulate representative input/output and storage failures; confirm the user receives a concise error message, no false success is shown, and the application remains usable when recovery is possible. Diagnostic details may be recorded for troubleshooting but shall not replace a clear user-facing message.

### NFR-003: Input Validation

The system shall validate console input at the point it is accepted and shall not rely solely on storage-layer or runtime failures to reject invalid values.

**Verification:** Exercise blank, whitespace-only, malformed, out-of-range, and unsupported menu inputs; confirm each is handled with corrective feedback and no invalid record is committed.

### NFR-004: Separation of Concerns

The implementation shall keep console interaction, student-management rules, and persistence responsibilities logically separate so that changes to one responsibility do not require unnecessary changes to the others.

**Verification:** A code review confirms that console prompts and formatting do not own persistence operations, and persistence code does not own interactive prompting or business validation decisions.

### NFR-005: Readable Console Output

The system shall present prompts, operation results, validation errors, and storage errors in plain language with consistent formatting.

**Verification:** For each supported workflow, a reviewer can identify the requested input, operation outcome, and any corrective action from the console output without inspecting logs or source code. Output shall remain understandable in a standard terminal without color support.

### NFR-006: Persistent Data Storage

The system shall store student records durably so that successfully saved additions, updates, and deletions are reflected after the application is closed and restarted.

**Verification:** Create, update, and delete records, restart the application after each operation, and confirm the stored state matches the last successfully completed operation. A failed write shall not be reported as successful; corrupted or unreadable data shall be reported rather than silently replaced with an empty store.

### NFR-007: Performance

For a local data set of up to 10,000 student records on a supported development-class computer, listing all records and searching by ID shall each complete within two seconds, excluding time spent waiting for operator input.

**Verification:** Measure operation time against a representative 10,000-record data set in a local environment and record the environment and results.

## 7. Assumptions and Open Decisions

- The initial release is intended for a single local operator; authentication, authorization, and concurrent editing are not required.
- The student ID format (allowed characters and maximum length) has not been specified and must be agreed before implementation. Regardless of format, IDs must be non-empty and unique under the rule in Section 4.
- The persistent storage technology and storage-file location have not been selected. The implementation must satisfy NFR-006 regardless of that choice.
- Email and phone are optional because no mandatory contact-information policy has been supplied.
- The application handles one student record per student ID; it does not define historical versions or soft deletion.

## 8. Traceability Summary

| Requirement | Capability |
|---|---|
| FR-001 | Add a new student |
| FR-002 | Display all students |
| FR-003 | Search student by ID |
| FR-004 | Update student details |
| FR-005 | Delete a student |
| FR-006 | Validate student information |
| FR-007 | Prevent duplicate student IDs |
| FR-008 | Provide menu navigation |
| NFR-001 | Maintainable code |
| NFR-002 | Proper exception handling |
| NFR-003 | Input validation |
| NFR-004 | Separation of concerns |
| NFR-005 | Readable console output |
| NFR-006 | Persistent data storage |
| NFR-007 | Performance |
