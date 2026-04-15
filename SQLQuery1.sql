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