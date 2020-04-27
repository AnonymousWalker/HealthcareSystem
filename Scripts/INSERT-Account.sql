USE [HealthcareSystem]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (1, N'abc@troy.edu', N'123456', N'Tony', N'Tran', N'3344655533', NULL, NULL, NULL, N'99999', N'201 University Avenue, Apt 302', NULL, 1)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (2, N'john@troy.edu', N'123456', N'John', N'Tran', N'12301230', N'999', 120000, 2, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (3, N'madison@troy.edu', N'123456', N'Madison', N'English', N'1113456340', N'7573', 25000, 2, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (4, N'Kelly@troy.edu', N'123456', N'Kelly', N'Anderson', N'1003876129', N'5213', 19000, 2, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (5, N'jamesh@troy.edu', N'123456', N'James', N'Henry', N'12333129', N'213', 9000, 1, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (6, N'nurse@troy.edu', N'123456', N'Kelly', N'Kim', N'12333129', N'213', 9000, 3, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (7, N'patient@troy.edu', N'123456', N'Sick', N'Patient', N'233471237', NULL, NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (8, N'CEO@troy.edu', N'123456', N'Staff', N'Unpaid', N'1237346', N'10139', 5000, 4, NULL, NULL, NULL, 2)
GO
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Appointment] ON 

GO
INSERT [dbo].[Appointment] ([AppointmentId], [Time], [DoctorId], [PatientId], [EmployeeAccount_AccountId], [PatientAccount_AccountId]) VALUES (1, CAST(N'2020-04-25 14:00:00.000' AS DateTime), 2, 1, NULL, NULL)
GO
INSERT [dbo].[Appointment] ([AppointmentId], [Time], [DoctorId], [PatientId], [EmployeeAccount_AccountId], [PatientAccount_AccountId]) VALUES (2, CAST(N'2020-04-25 15:00:00.000' AS DateTime), 2, 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Appointment] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicalRecord] ON 

GO
INSERT [dbo].[MedicalRecord] ([Id], [Weight], [Height], [BloodPressure], [Pulse], [Description], [Date], [LabResult], [RadiologyReport], [PathologyReport], [AllegyInformation], [PatientId], [PatientAccount_AccountId]) VALUES (1, 999, 999, N'111/99', 96, N' Regular Check', CAST(N'2020-04-26 00:00:00.000' AS DateTime), N'none', NULL, N'none', NULL, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[MedicalRecord] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceFee] ON 

GO
INSERT [dbo].[ServiceFee] ([ServiceId], [ServiceName], [Fee]) VALUES (1, N'Blood Test', 35)
GO
INSERT [dbo].[ServiceFee] ([ServiceId], [ServiceName], [Fee]) VALUES (2, N'X-Ray Scan', 20)
GO
INSERT [dbo].[ServiceFee] ([ServiceId], [ServiceName], [Fee]) VALUES (3, N'MRI Scan', 78)
GO
INSERT [dbo].[ServiceFee] ([ServiceId], [ServiceName], [Fee]) VALUES (4, N'Radiology', 120)
GO
INSERT [dbo].[ServiceFee] ([ServiceId], [ServiceName], [Fee]) VALUES (5, N'Lab Test', 50)
GO
INSERT [dbo].[ServiceFee] ([ServiceId], [ServiceName], [Fee]) VALUES (6, N'Appointment', 50)
GO
SET IDENTITY_INSERT [dbo].[ServiceFee] OFF
GO
