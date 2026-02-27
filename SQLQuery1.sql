CREATE DATABASE TelemedicineDB;
GO
USE TelemedicineDB;

CREATE TABLE Doctors (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Specialty NVARCHAR(100),
    IsActive BIT DEFAULT 1
);

CREATE TABLE Patients (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(20)
);

CREATE TABLE Appointments (
    Id INT PRIMARY KEY IDENTITY,
    DoctorId INT NOT NULL,
    PatientId INT NOT NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Scheduled',
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Appointments_Doctor 
        FOREIGN KEY (DoctorId) REFERENCES Doctors(Id),

    CONSTRAINT FK_Appointments_Patient 
        FOREIGN KEY (PatientId) REFERENCES Patients(Id),

    CONSTRAINT CK_Appointments_Time
        CHECK (StartTime < EndTime),

    CONSTRAINT CK_Appointments_Status
        CHECK (Status IN ('Scheduled','Completed','Cancelled'))
);









CREATE INDEX Appointments_Doctor_Time
ON Appointments (DoctorId, StartTime, EndTime);

CREATE TABLE WaitingRooms (
    Id INT PRIMARY KEY IDENTITY,
    AppointmentId INT NOT NULL,
    IsPatientJoined BIT DEFAULT 0,
    JoinTime DATETIME2,

    CONSTRAINT FK_WaitingRooms_Appointments
        FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id)
);

ALTER TABLE Appointments
ADD IsDeleted BIT NOT NULL DEFAULT 0;

ALTER TABLE Appointments
ADD DeletedAt DATETIME2 NULL;

INSERT INTO Doctors (FullName, Email, Specialty, IsActive)
VALUES 
('Dr. Ahmed Hassan', 'ahmed.hassan@clinic.com', 'Cardiology', 1),
('Dr. Mohamed Ali', 'mohamed.ali@clinic.com', 'Dermatology', 1),
('Dr. Sara Khaled', 'sara.khaled@clinic.com', 'Pediatrics', 1),
('Dr. Omar Youssef', 'omar.youssef@clinic.com', 'Neurology', 1);

INSERT INTO Patients (FullName, Email, Phone)
VALUES
('Ali Mahmoud', 'ali.mahmoud@gmail.com', '01012345678'),
('Mona Adel', 'mona.adel@gmail.com', '01098765432'),
('Youssef Tarek', 'youssef.tarek@gmail.com', '01122334455'),
('Nada Samir', 'nada.samir@gmail.com', '01233445566'),
('Khaled Fathy', 'khaled.fathy@gmail.com', '01566778899');

INSERT INTO Appointments 
(DoctorId, PatientId, StartTime, EndTime, Status)
VALUES

-- Doctor 1
(1, 1, '2026-02-18 09:00:00', '2026-02-18 09:30:00', 'Scheduled'),
(1, 2, '2026-02-18 10:00:00', '2026-02-18 10:30:00', 'Completed'),
(1, 3, '2026-02-18 11:00:00', '2026-02-18 11:30:00', 'Cancelled'),

-- Doctor 2
(2, 4, '2026-02-18 09:00:00', '2026-02-18 09:30:00', 'Scheduled'),
(2, 5, '2026-02-18 10:00:00', '2026-02-18 10:30:00', 'Scheduled'),

-- Doctor 3
(3, 1, '2026-02-18 12:00:00', '2026-02-18 12:30:00', 'Completed'),

-- Doctor 4
(4, 2, '2026-02-18 13:00:00', '2026-02-18 13:45:00', 'Scheduled');

INSERT INTO WaitingRooms (AppointmentId, IsPatientJoined, JoinTime)
VALUES
(1, 0, NULL),
(2, 1, '2026-02-18 09:55:00'),
(4, 0, NULL),
(5, 1, '2026-02-18 09:58:00');




ALTER TABLE Appointments
ALTER COLUMN Status INT NOT NULL;

ALTER TABLE Appointments
ADD CONSTRAINT DF_Appointments_Status DEFAULT 1 FOR Status;


ALTER TABLE Appointments
ADD CONSTRAINT CK_Appointments_Status
CHECK (Status IN (1, 2, 3));

ALTER TABLE Appointments
ADD RowVersion ROWVERSION NOT NULL;


CREATE TABLE Users (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    Email    NVARCHAR(150) NOT NULL UNIQUE,
    Password NVARCHAR(256) NOT NULL,
    Role     NVARCHAR(50)  NOT NULL DEFAULT 'Patient',
    RefId    INT           NOT NULL,
    CONSTRAINT CK_Users_Role 
        CHECK (Role IN ('Doctor', 'Patient', 'Admin'))
);