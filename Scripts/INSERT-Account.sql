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
