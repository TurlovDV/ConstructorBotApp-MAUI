using AutoMapper;
using Core.DomainHandler;
using Core.UserModel;
using Model.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Telegram;

namespace Core
{
    public class Domain
    {
        public List<DomainUser> Users = null!;

        public Domain(List<DomainUser> users)
        {
            Users = users;
        }

        public Domain()
        {
            Users = new List<DomainUser>();
        }
        public void AddUser(User user)
        {
            Users.RemoveAll(x => x.Id == user.Id);
            
            List<DomainEntityBot> newDomainEntityBots = new List<DomainEntityBot>();
            foreach (var item in user.Bots)
            {
                DomainEntityBot newEntity = new DomainEntityBot() { Id = item.Id,  Logger = new Logger() };
                newEntity.Handler = new Handler(item);

                //newEntity.Logger.Logs.Add(new Log() { Status = })

                newDomainEntityBots.Add(newEntity);
            }

            DomainUser newUser = new DomainUser()
            {
                Id = user.Id,
                Bots = newDomainEntityBots
            };
            
            Users.Add(newUser);
        }


        public void PutUser(User user)
        {
            DomainUser domainUser = new DomainUser();
            foreach (var item in Users)
                if (item.Id == user.Id)
                    domainUser = item;
            if(Users.Remove(domainUser))
                AddUser(user);
        }

        public bool IsUser(User user)
        {
            foreach (var item in Users)
                if (item.Id == user.Id)
                    return true;
            return false;
        }
    }
}
