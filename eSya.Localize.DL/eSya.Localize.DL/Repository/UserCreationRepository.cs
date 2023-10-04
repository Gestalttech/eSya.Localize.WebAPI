using eSya.Localize.DL.Entities;
using eSya.Localize.DL.Repository;
using eSya.Localize.DO;
using eSya.Localize.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Localize.DL.Repository
{
   public class UserCreationRepository: IUserCreationRepository
    {
        private readonly IStringLocalizer<UserCreationRepository> _localizer;
        public UserCreationRepository(IStringLocalizer<UserCreationRepository> localizer)
        {
            _localizer = localizer;
        }
        public async Task<List<DO_UserCreation>> GetAllUsers()
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    return await db.GtEcltus
                           .Select(u => new DO_UserCreation
                           {
                               UserId = u.UserId,
                               LoginId = u.LoginId,
                               LoginDesc = u.LoginDesc,
                               Password = eSyaCryptGeneration.Decrypt(u.Password),
                               ActiveStatus = u.ActiveStatus
                           }).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateUserCreation(DO_UserCreation obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEcltu _user = db.GtEcltus.Where(x => x.UserId == obj.UserId).FirstOrDefault();

                        if (_user == null)
                        {
                            GtEcltu is_userExists = db.GtEcltus.FirstOrDefault(k => k.LoginId.ToUpper().Replace(" ", "") == obj.LoginId.ToUpper().Replace(" ", ""));
                            if (is_userExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0111", Message = string.Format(_localizer[name: "W0111"]) };
                            }
                            int maxval = db.GtEcltus.Select(c => c.UserId).DefaultIfEmpty().Max();
                            int _userId = maxval + 1;
                            _user = new GtEcltu
                            {
                                UserId = _userId,
                                LoginId = obj.LoginId,
                                LoginDesc = obj.LoginDesc,
                                Password = eSyaCryptGeneration.Encrypt(obj.Password),
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.User_ID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcltus.Add(_user);
                        }
                        else
                        {
                            GtEcltu _isuserExists = db.GtEcltus.FirstOrDefault(k => k.LoginId.ToUpper().Replace(" ", "") == obj.LoginId.ToUpper().Replace(" ", "") && k.UserId != obj.UserId);
                            if (_isuserExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0111", Message = string.Format(_localizer[name: "W0111"]) };

                            }
                            _user.LoginId = obj.LoginId;
                            _user.LoginDesc = obj.LoginDesc;
                            _user.Password = eSyaCryptGeneration.Encrypt(obj.Password);
                            _user.ActiveStatus = obj.ActiveStatus;
                            _user.ModifiedBy = obj.User_ID;
                            _user.ModifiedOn = System.DateTime.Now;
                            _user.ModifiedTerminal = obj.TerminalID;
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();


                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }

                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }

        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveUser(bool status, int UserId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcltu _user = db.GtEcltus.Where(w => w.UserId == UserId).FirstOrDefault();
                        if (_user == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0112", Message = string.Format(_localizer[name: "W0112"]) };
                        }

                        _user.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
