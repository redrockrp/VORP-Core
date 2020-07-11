using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_sv.Class
{
    public class Character : BaseScript
    {
        private string identifier;

        private string group;
        private string job;
        private string jobgrade;
        private string firstname;
        private string lastname;

        private double money;
        private double gold;
        private double rol;

        private int xp;

        public string Identifier { get => identifier; }
        public string Group { get => group; }
        public string Job { get => job; }
        public string Jobgrade { get => jobgrade; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public double Money { get => money; }
        public double Gold { get => gold; }
        public double Rol { get => rol; }
        public int Xp { get => xp; }

        public Character()
        {

        }

        public Character(string identifier, string group, string job, string jobgrade, string firstname, string lastname, double money, double gold, double rol, int xp)
        {
            this.identifier = identifier;
            this.group = group;
            this.job = job;
            this.jobgrade = jobgrade;
            this.firstname = firstname;
            this.lastname = lastname;
            this.money = money;
            this.gold = gold;
            this.rol = rol;
            this.xp = xp;
        }

        public void addCurrency(int currency, double quantity)
        {
            switch (currency)
            {
                case 0:
                    money += quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET money=money + {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 1:
                    gold += quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET gold=gold + {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 2:
                    rol += quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET rol=rol + {quantity} WHERE identifier=?", new[] { identifier });
                    break;
            }
        }

        public void removeCurrency(int currency, double quantity)
        {
            switch (currency)
            {
                case 0:
                    money -= quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET money=money - {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 1:
                    gold -= quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET gold=gold - {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 2:
                    rol -= quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET rol=rol - {quantity} WHERE identifier=?", new[] { identifier });
                    break;
            }
        }

        public void addXp(int quantity)
        {
            xp += quantity;
            Exports["ghmattimysql"].execute($"UPDATE characters SET xp=xp + {quantity} WHERE identifier=?", new[] { identifier });
        }

        public void removeXp(int quantity)
        {
            xp -= quantity;
            Exports["ghmattimysql"].execute($"UPDATE characters SET xp=xp - {quantity} WHERE identifier=?", new[] { identifier });
        }

        public void setJob(string newjob)
        {
            job = newjob;
            Exports["ghmattimysql"].execute($"UPDATE characters SET job={newjob} WHERE identifier=?", new[] { identifier });
        }

        public void setGroup(string newgroup)
        {
            group = newgroup;
            Exports["ghmattimysql"].execute($"UPDATE characters SET group={newgroup} WHERE identifier=?", new[] { identifier });
        }

    }
}
