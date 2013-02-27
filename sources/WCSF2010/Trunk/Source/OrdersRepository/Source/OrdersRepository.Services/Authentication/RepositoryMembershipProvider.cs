//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory 2010
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Web.Security;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.Services.Properties;
using OrdersRepository.Services.Utility;

namespace OrdersRepository.Services.Authentication
{
    public class RepositoryMembershipProvider : MembershipProvider
    {
		IEmployeeService _employeeService;

		public IEmployeeService EmployeeService
		{
			get { return _employeeService; }
			set { _employeeService = value; }
		}

		private string _validPassword;

		public string ValidPassword
		{
			get { return _validPassword; }
			set { _validPassword = value; }
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public override bool ValidateUser(string username, string password)
        {
            Guard.ArgumentNotNullOrEmptyString(username, "username");

            Employee employee = EmployeeService.GetEmployeeById(username.ToLowerInvariant());
            return employee != null && ValidPassword == password;
        }

        #region Not implemented methods

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
            set
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        #endregion
    }
}
