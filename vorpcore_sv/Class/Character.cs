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
        string identifier;

        string group;
        string job;
        string jobgrade;
        string firstname;
        string lastname;

        double money;
        double gold;
        double rol;

        int xp;

        public Character(string identifier, string group, string job, string jobgrade, string firstname, string lastname, double money, double gold, double rol, int xp)
        {
            this.Identifier = identifier;
            this.Group = group;
            this.Job = job;
            this.Jobgrade = jobgrade;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Money = money;
            this.Gold = gold;
            this.Rol = rol;
            this.Xp = xp;
        }

        public void addCurrency(int currency, double quantity)
        {
            switch (currency)
            {
                case 0:
                    this.money += quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET money=money + {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 1:
                    this.gold += quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET gold=gold + {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 2:
                    this.rol += quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET rol=rol + {quantity} WHERE identifier=?", new[] { identifier });
                    break;
            }
        }

        public void removeCurrency(int currency, double quantity)
        {
            switch (currency)
            {
                case 0:
                    this.money -= quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET money=money - {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 1:
                    this.gold -= quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET gold=gold - {quantity} WHERE identifier=?", new[] { identifier });
                    break;
                case 2:
                    this.rol -= quantity;
                    Exports["ghmattimysql"].execute($"UPDATE characters SET rol=rol - {quantity} WHERE identifier=?", new[] { identifier });
                    break;
            }
        }

        public string Identifier { get => identifier; set => identifier = value; }
        public string Group { get => group; set => group = value; }
        public string Job { get => job; set => job = value; }
        public string Jobgrade { get => jobgrade; set => jobgrade = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public double Money { get => money; set => money = value; }
        public double Gold { get => gold; set => gold = value; }
        public double Rol { get => rol; set => rol = value; }
        public int Xp { get => xp; set => xp = value; }
    }
}
