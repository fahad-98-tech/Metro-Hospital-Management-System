SELECT * FROM dbo.Doctors

use MetroHospitalDB


ALTER TABLE Doctors
ADD FailedAttempts INT NOT NULL DEFAULT 0,
    LockUntil DATETIME NULL;