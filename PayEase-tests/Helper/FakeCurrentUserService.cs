using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayEase_CaseStudy.Services;

namespace PayEase_tests.Helper
{
    public class FakeCurrentUserService : ICurrentUserService
    {
        private readonly string _userId;

        public FakeCurrentUserService(string userId)
        {
            _userId = userId;
        }

        public string GetUserId()
        {
            return _userId;
        }
    }
}
