-- =========================================
-- CREATE DATABASE
-- =========================================
CREATE DATABASE MetroHospitalDB;
GO

USE MetroHospitalDB;
GO

select * from Notifications
CREATE TABLE Notifications
(
    NotificationId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL,
    AppointmentId INT NULL,
    Message NVARCHAR(500) NOT NULL,
    Status NVARCHAR(10) NOT NULL DEFAULT 'Unread',
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (PatientId) REFERENCES Users(UserId),
    CONSTRAINT FK_Notifications_Appointments FOREIGN KEY (AppointmentId) REFERENCES Appointments(AppointmentId)
);




GO


-- =========================================
-- 2. DOCTORS TABLE
-- =========================================
CREATE TABLE Doctors (
    DoctorId INT IDENTITY(1,1) PRIMARY KEY,

    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    MobileNumber NVARCHAR(20) NOT NULL,

    Gender NVARCHAR(10) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Age INT NOT NULL,

    PasswordHash NVARCHAR(256) NOT NULL,

    Specialization NVARCHAR(150) NOT NULL,

    DoctorImage NVARCHAR(250) NULL,

    CreatedBy INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Doctors_CreatedBy
        FOREIGN KEY (CreatedBy)
        REFERENCES Users(UserId)
);
GO


-- =========================================
-- 3. APPOINTMENTS TABLE
-- =========================================
CREATE TABLE Appointments (
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,

    DoctorId INT NOT NULL,
    PatientId INT NULL,

    PatientName NVARCHAR(150) NOT NULL,
    PatientMobile NVARCHAR(20) NOT NULL,

    AppointmentDate DATE NOT NULL,

    AppointmentTime VARCHAR(5) NOT NULL,
    AppointmentEndTime VARCHAR(5) NOT NULL,

    Specialization NVARCHAR(150) NOT NULL,

    Status NVARCHAR(50) NOT NULL DEFAULT 'Booked',

    DischargeStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending',

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Appointments_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(DoctorId),

    CONSTRAINT FK_Appointments_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Users(UserId)
);
GO


-- =========================================
-- 4. APPOINTMENT TREATMENTS
-- =========================================
CREATE TABLE AppointmentTreatments
(
    TreatmentId INT IDENTITY(1,1) PRIMARY KEY,

    AppointmentId INT NOT NULL,
    DoctorId INT NOT NULL,

    Symptoms NVARCHAR(MAX) NULL,
    Diagnosis NVARCHAR(MAX) NULL,
    Notes NVARCHAR(MAX) NULL,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_AppointmentTreatments_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(AppointmentId),

    CONSTRAINT FK_AppointmentTreatments_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(DoctorId)
);
GO


-- =========================================
-- 5. MEDICINES MASTER TABLE
-- =========================================
CREATE TABLE Medicines
(
    MedicineId INT IDENTITY(1,1) PRIMARY KEY,

    MedicineName NVARCHAR(150) NOT NULL,
    Specialization NVARCHAR(150) NULL,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO


-- =========================================
-- 6. APPOINTMENT MEDICINES
-- =========================================
CREATE TABLE AppointmentMedicines
(
    MedicineId INT IDENTITY(1,1) PRIMARY KEY,

    AppointmentId INT NOT NULL,

    MedicineName NVARCHAR(150) NOT NULL,
    Dosage NVARCHAR(50) NULL,
    Duration NVARCHAR(50) NULL,

    Instructions NVARCHAR(MAX) NULL,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AppointmentMedicines_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(AppointmentId)
);
GO

select * from TestCancelFeedback
CREATE TABLE TestCancelFeedback (
    FeedbackId INT IDENTITY(1,1) PRIMARY KEY,

    PatientTestId INT NOT NULL,   -- ✅ लिंक to PatientTest
    PatientId INT NOT NULL,

    CancelReason NVARCHAR(255),
    FeedbackText NVARCHAR(MAX),

    CreatedDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (PatientTestId) REFERENCES PatientTests(PatientTestId),
    FOREIGN KEY (PatientId) REFERENCES users(userid)
);
-- =========================================
-- 7. APPOINTMENT REPORTS
-- =========================================
CREATE TABLE AppointmentReports
(
    ReportId INT IDENTITY(1,1) PRIMARY KEY,

    AppointmentId INT NOT NULL,

    ReportType NVARCHAR(150) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,

    UploadedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AppointmentReports_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(AppointmentId)
);
GO


-- =========================================
-- 8. INVOICES
-- =========================================
CREATE TABLE Invoices
(
    InvoiceId INT IDENTITY(1,1) PRIMARY KEY,

    AppointmentId INT NOT NULL,

    ConsultationFee DECIMAL(18,2) NOT NULL DEFAULT 0,
    TestCharges DECIMAL(18,2) NOT NULL DEFAULT 0,
    MedicineCharges DECIMAL(18,2) NOT NULL DEFAULT 0,

    PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Invoices_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(AppointmentId)
);
GO


-- =========================================
-- 9. NOTIFICATIONS
-- =======================
select * from Appointments
select * from Notifications
delete from Notifications

 SELECT a.AppointmentId, a.DoctorId, a.AppointmentDate, a.AppointmentTime, a.AppointmentEndTime, d.FullName
            FROM Appointments a
            INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
            WHERE a.PatientId=8
              AND a.IsActive = 1 and a.Status!='Cancelled'

==================
CREATE TABLE Notifications
(
    NotificationId INT IDENTITY(1,1) PRIMARY KEY,

    PatientId INT NOT NULL,
    AppointmentId INT NULL,

    Message NVARCHAR(500) NOT NULL,

    IsRead BIT NOT NULL DEFAULT 0,

    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Notifications_Users
        FOREIGN KEY (PatientId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Notifications_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(AppointmentId)
);
GO













use MetroHospitalDB



-- Make columns large enough to store times like "6:00 PM"
ALTER TABLE Appointments
ALTER COLUMN AppointmentTime VARCHAR(8) NOT NULL;

ALTER TABLE Appointments
ALTER COLUMN AppointmentEndTime VARCHAR(8) NOT NULL;



CREATE TABLE DoctorShifts (
    ShiftId INT IDENTITY(1,1) PRIMARY KEY,
    DoctorId INT NOT NULL,
    ShiftDate DATE NOT NULL,
    ShiftStart TIME NOT NULL,
    ShiftEnd TIME NOT NULL,
    ShiftType NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,

    -- Optional: Foreign Key (if Doctor table exists)
    CONSTRAINT FK_DoctorShifts_Doctor
    FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId)
);



CREATE TABLE Tests (
    TestId INT IDENTITY(1,1) PRIMARY KEY,
    TestName NVARCHAR(100) NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);



CREATE TABLE PatientTests (
    PatientTestId INT IDENTITY(1,1) PRIMARY KEY,
    
    PatientId INT NOT NULL,
    DoctorId INT NULL,
    AppointmentId INT NULL,
    TestId INT NOT NULL,
    
    TestDate DATE NOT NULL,
    TestTime TIME NOT NULL,
    
    PatientName NVARCHAR(100) NOT NULL,
    Contact NVARCHAR(20) NOT NULL,
    
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',

    -- Optional Foreign Keys
   

    CONSTRAINT FK_PatientTests_Doctor 
        FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId),

    CONSTRAINT FK_PatientTests_Appointment 
        FOREIGN KEY (AppointmentId) REFERENCES Appointments(AppointmentId),

    CONSTRAINT FK_PatientTests_Test 
        FOREIGN KEY (TestId) REFERENCES Tests(TestId)
);


ALTER TABLE PatientTests
ADD Result NVARCHAR(255) NULL;


CREATE TABLE TestPayments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    
    PatientTestId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    
    PaymentMode NVARCHAR(50) NOT NULL,         -- e.g., Cash, Card, Online
    TransactionDetails NVARCHAR(255) NULL,     -- Optional details
    
    PaymentDate DATE NOT NULL,

    -- Optional Foreign Key
    CONSTRAINT FK_TestPayments_PatientTest
        FOREIGN KEY (PatientTestId) REFERENCES PatientTests(PatientTestId)
);



CREATE TABLE TestCancelFeedback (
    FeedbackId INT IDENTITY(1,1) PRIMARY KEY,

    PatientTestId INT NOT NULL,   -- ✅ लिंक to PatientTest
    PatientId INT NOT NULL,

    CancelReason NVARCHAR(255),
    FeedbackText NVARCHAR(MAX),

    CreatedDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (PatientTestId) REFERENCES PatientTests(PatientTestId),
    FOREIGN KEY (PatientId) REFERENCES users(userid)
);