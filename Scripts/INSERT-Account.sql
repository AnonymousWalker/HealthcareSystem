USE [HealthcareSystem]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (1, N'abc@troy.edu', N'123456', N'Anh', N'Tran', N'3344655533', NULL, NULL, NULL, N'99999', N'201 University Avenue, Apt 302', NULL, 1)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (2, N'john@troy.edu', N'123456', N'John', N'Tran', N'12301230', N'999', 1, 2, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (3, N'madison@troy.edu', N'123456', N'Madison', N'English', N'1113456340', N'7573', 25000.5, 2, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [Firstname], [Lastname], [Phone], [SSN], [Salary], [Role], [InsuranceNumber], [BillingAddress], [DayOfBirth], [AccountType]) VALUES (4, N'Kelly@troy.edu', N'123456', N'Kelly', N'Anderson', N'1003876129', N'5213', 191923, 2, NULL, NULL, NULL, 2)
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
INSERT [dbo].[Appointment] ([AppointmentId], [Time], [DoctorId], [PatientId], [EmployeeAccount_AccountId], [PatientAccount_AccountId]) VALUES (3, CAST(N'2020-04-28 10:00:00.000' AS DateTime), 2, 1, NULL, NULL)
GO
INSERT [dbo].[Appointment] ([AppointmentId], [Time], [DoctorId], [PatientId], [EmployeeAccount_AccountId], [PatientAccount_AccountId]) VALUES (4, CAST(N'2020-04-28 14:00:00.000' AS DateTime), 2, 1, NULL, NULL)
GO
INSERT [dbo].[Appointment] ([AppointmentId], [Time], [DoctorId], [PatientId], [EmployeeAccount_AccountId], [PatientAccount_AccountId]) VALUES (5, CAST(N'2020-04-28 14:00:00.000' AS DateTime), 3, 1, NULL, NULL)
GO
INSERT [dbo].[Appointment] ([AppointmentId], [Time], [DoctorId], [PatientId], [EmployeeAccount_AccountId], [PatientAccount_AccountId]) VALUES (6, CAST(N'2020-04-28 14:00:00.000' AS DateTime), 4, 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Appointment] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicalRecord] ON 

GO
INSERT [dbo].[MedicalRecord] ([Id], [Weight], [Height], [BloodPressure], [Pulse], [Description], [Date], [LabResult], [RadiologyReport], [PathologyReport], [AllegyInformation], [PatientId], [PatientAccount_AccountId]) VALUES (1, 999, 999, N'111/99', 96, N' Regular Check', CAST(N'2020-04-26 00:00:00.000' AS DateTime), N'none', NULL, N'none', NULL, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[MedicalRecord] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceStatement] ON 

GO
INSERT [dbo].[ServiceStatement] ([Id], [Prescription], [Date], [Amount], [Status], [DoctorId], [AppointmentId], [PatientId], [PatientAccount_AccountId]) VALUES (1, N'Paracetamol 500mg', CAST(N'2020-04-27 18:37:19.063' AS DateTime), 50, 1, 2, 3, 1, NULL)
GO
INSERT [dbo].[ServiceStatement] ([Id], [Prescription], [Date], [Amount], [Status], [DoctorId], [AppointmentId], [PatientId], [PatientAccount_AccountId]) VALUES (2, N'Full scan', CAST(N'2020-04-27 18:37:05.907' AS DateTime), 50, 0, 2, 4, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[ServiceStatement] OFF
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
SET IDENTITY_INSERT [dbo].[ServiceStatementDetail] ON 

GO
INSERT [dbo].[ServiceStatementDetail] ([Id], [StatementId], [ServiceId]) VALUES (12, 2, 6)
GO
INSERT [dbo].[ServiceStatementDetail] ([Id], [StatementId], [ServiceId]) VALUES (13, 1, 6)
GO
SET IDENTITY_INSERT [dbo].[ServiceStatementDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[DailyReport] ON 

GO
INSERT [dbo].[DailyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (19, 2, 100, CAST(N'2020-04-27 20:30:06.547' AS DateTime))
GO
INSERT [dbo].[DailyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (20, 3, 0, CAST(N'2020-04-27 20:30:06.547' AS DateTime))
GO
INSERT [dbo].[DailyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (21, 4, 0, CAST(N'2020-04-27 20:30:06.547' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[DailyReport] OFF
GO
SET IDENTITY_INSERT [dbo].[MonthlyReport] ON 

GO
INSERT [dbo].[MonthlyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (7, 2, 100, CAST(N'2020-04-27 20:36:26.503' AS DateTime))
GO
INSERT [dbo].[MonthlyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (8, 3, 0, CAST(N'2020-04-27 20:36:26.503' AS DateTime))
GO
INSERT [dbo].[MonthlyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (9, 4, 0, CAST(N'2020-04-27 20:36:26.503' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MonthlyReport] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

GO
INSERT [dbo].[Transaction] ([TransactionId], [StatementId], [Amount], [Status], [Date], [PaymentMethod], [PaymentNumber], [PayerName], [BillingAddress], [ReferenceNumber]) VALUES (1, 1, 50, N'Pending', CAST(N'2020-04-27 19:12:29.483' AS DateTime), 2, N'912312', N'AT', N'Troy', N'')
GO
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO

SET IDENTITY_INSERT [dbo].[DailyReport] ON 

GO
INSERT [dbo].[DailyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (28, 2, 0, CAST(N'2020-04-28 10:15:29.503' AS DateTime))
GO
INSERT [dbo].[DailyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (29, 3, 0, CAST(N'2020-04-28 10:15:29.503' AS DateTime))
GO
INSERT [dbo].[DailyReport] ([Id], [DoctorId], [Revenue], [Date]) VALUES (30, 4, 0, CAST(N'2020-04-28 10:15:29.503' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[DailyReport] OFF
GO
