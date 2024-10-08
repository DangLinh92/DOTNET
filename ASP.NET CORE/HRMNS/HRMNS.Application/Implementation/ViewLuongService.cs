using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.luong;
using HRMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class ViewLuongService : BaseService, IViewLuongService
    {
        IUnitOfWork _unitOfWork;
        IRespository<USER_VIEW_LUONG, Guid> _userViewLuongRepository;
        public ViewLuongService(IUnitOfWork unitOfWork, IRespository<USER_VIEW_LUONG, Guid> userViewLuongRepository)
        {
            _unitOfWork = unitOfWork;
            _userViewLuongRepository = userViewLuongRepository;
        }

        public USER_VIEW_LUONG GetUserByName(string name)
        {
            return _userViewLuongRepository.FindSingle(x => x.UserName == name);
        }

        public bool ChangePassword(string userName, string password, string newpassword)
        {
            USER_VIEW_LUONG user = _userViewLuongRepository.FindSingle(x => x.UserName == userName);

            if (user != null)
            {
                if (user.FirtLogin > 0)
                {
                    if (user.Password == password)
                    {
                        user.Password = newpassword;
                        _userViewLuongRepository.Update(user);
                        return true;
                    }
                }
                else
                {
                    if (user.PasswordDefault == password)
                    {
                        user.Password = newpassword;
                        user.FirtLogin = 1;

                        _userViewLuongRepository.Update(user);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Login(string userName, string password)
        {
            USER_VIEW_LUONG user = _userViewLuongRepository.FindSingle(x => x.UserName == userName);

            if (user != null)
            {
                if (user.FirtLogin > 0)
                {
                    return user.Password == password;
                }
                else
                {
                    return user.PasswordDefault == password;
                }
            }

            return false;
        }

        public bool Logout()
        {
            return true;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
