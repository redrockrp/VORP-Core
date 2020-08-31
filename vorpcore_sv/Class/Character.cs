using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_sv.Class
{
    //Class for user characters
    public class Character : BaseScript
    {
        private string identifier;
        private int charIdentifier;
        private string group;
        private string job;
        private int jobgrade;
        private string firstname;
        private string lastname;
        private string inventory;
        private string status;
        private string coords;
        private string skin;
        private string comps;

        private double money;
        private double gold;
        private double rol;

        private int xp;

        private bool isdead;

        private bool SaveCharacter;

        public string Identifier { get => identifier; }
        public int CharIdentifier { get => charIdentifier; set => charIdentifier = value; }
        public string Group { get => group; }
        public string Job { get => job; }
        public int Jobgrade { get => jobgrade; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Inventory { get => inventory; set => inventory = value; }
        public string Status { get => status; set => status = value; }
        public string Coords { get => coords; set => coords = value; }
        public double Money { get => money; }
        public double Gold { get => gold; }
        public double Rol { get => rol; }
        public int Xp { get => xp; }
        public bool IsDead { get => isdead; }

        public string Skin
        {
            get => skin;
            set
            {
                skin = value;
                Exports["ghmattimysql"].execute("UPDATE characters SET `skinPlayer` = ? WHERE `identifier` = ? AND `charidentifier` = ?", new object[] { value, Identifier,CharIdentifier });
            }
        }
        
        public string Comps
        {
            get => comps;
            set
            {
                comps = value;
                Exports["ghmattimysql"].execute("UPDATE characters SET `compPlayer` = ? WHERE `identifier` = ? AND `charidentifier` = ?", new object[] { value, Identifier,CharIdentifier });
            }
        }

        public Character(string identifier)
        {
            this.identifier = identifier;
            this.job = "unemployed";
            this.group = "user";
            this.inventory = "{}";
        }

        public Character(string identifier, int charIdentifier ,string group, string job, int jobgrade, string firstname, string lastname, string inventory, string status, string coords, double money, double gold, double rol, int xp, bool isdead,string skin,string comps)
        {
            this.identifier = identifier;
            this.charIdentifier = charIdentifier;
            this.group = group;
            this.job = job;
            this.jobgrade = jobgrade;
            this.firstname = firstname;
            this.lastname = lastname;
            this.inventory = inventory;
            this.status = status;
            this.coords = coords;
            this.money = money;
            this.gold = gold;
            this.rol = rol;
            this.xp = xp;
            this.isdead = isdead;
            this.skin = skin;
            this.comps = comps;
            SaveCharacter = false;
        }

        public Dictionary<string, dynamic> getCharacter()
        {
            Dictionary<string, dynamic> userData = new Dictionary<string,dynamic>();
            userData.Add("identifier", identifier);
            userData.Add("group", group);
            userData.Add("job", job);
            userData.Add("money", money);
            userData.Add("gold", gold);
            userData.Add("rol", rol);
            userData.Add("xp", xp);
            userData.Add("firstname", firstname);
            userData.Add("lastname", lastname);
            userData.Add("inventory", inventory);
            userData.Add("status", status);
            userData.Add("coords", coords);
            userData.Add("isdead", isdead);
            userData.Add("skin",skin);
            userData.Add("setGroup", new Action<string>((g) =>
            {
                group = g;
            }));
            userData.Add("setJob", new Action<string>((j) =>
            {
                job = j;
            }));
            userData.Add("setMoney", new Action<double>((m) =>
            {
                money = m;
            }));
            userData.Add("setGold", new Action<double>((g) =>
            {
                gold = g;
            }));
            userData.Add("setRol", new Action<double>((r) =>
            {
                rol = r;
            }));
            userData.Add("setXp", new Action<int>((x) =>
            {
                xp = x;
            }));
            userData.Add("setFirstname", new Action<string>((f) =>
            {
                firstname = f;
            }));
            userData.Add("setLastname", new Action<string>((l) =>
            {
                lastname = l;
            }));
            userData.Add("updateSkin",new Action<string>((nskin) =>
            {
                Skin = nskin;
            }));
            userData.Add("updateComps",new Action<string>((ncomps) =>
            {
                Comps = ncomps;
            }));
            return userData;
        }

        public void addCurrency(int currency, double quantity)
        {
            switch (currency)
            {
                case 0:
                    money += quantity;
                    SaveCharacter = true;
                    //Exports["ghmattimysql"].execute($"UPDATE characters SET money=money + ? WHERE identifier=?", new object[] { quantity, identifier });
                    break;
                case 1:
                    gold += quantity;
                    SaveCharacter = true;
                    //Exports["ghmattimysql"].execute($"UPDATE characters SET gold=gold + ? WHERE identifier=?", new object[] { quantity, identifier });
                    break;
                case 2:
                    rol += quantity;
                    SaveCharacter = true;
                    //Exports["ghmattimysql"].execute($"UPDATE characters SET rol=rol + ? WHERE identifier=?", new object[] { quantity, identifier });
                    break;
            }
        }

        public void removeCurrency(int currency, double quantity)
        {
            switch (currency)
            {
                case 0:
                    money -= quantity;
                    SaveCharacter = true;
                    //Exports["ghmattimysql"].execute($"UPDATE characters SET money=money - ? WHERE identifier=?", new object[] { quantity, identifier });
                    break;
                case 1:
                    gold -= quantity;
                    SaveCharacter = true;
                    //Exports["ghmattimysql"].execute($"UPDATE characters SET gold=gold - ? WHERE identifier=?", new object[] { quantity, identifier });
                    break;
                case 2:
                    rol -= quantity;
                    SaveCharacter = true;
                    //Exports["ghmattimysql"].execute($"UPDATE characters SET rol=rol - ? WHERE identifier=?", new object[] { quantity, identifier });
                    break;
            }
        }

        public void addXp(int quantity)
        {
            xp += quantity;
            SaveCharacter = true;
            //Exports["ghmattimysql"].execute($"UPDATE characters SET xp=xp + ? WHERE identifier=?", new object[] { quantity, identifier });
        }

        public void removeXp(int quantity)
        {
            xp -= quantity;
            SaveCharacter = true;
            //Exports["ghmattimysql"].execute($"UPDATE characters SET xp=xp - ? WHERE identifier=?", new object[] { quantity, identifier });
        }

        public void setJob(string newjob)
        {
            job = newjob;
            SaveCharacter = true;
            //Exports["ghmattimysql"].execute($"UPDATE characters SET job=? WHERE identifier=?", new string[] { newjob, identifier });
        }

        public void setGroup(string newgroup)
        {
            group = newgroup;
            SaveCharacter = true;
            //Exports["ghmattimysql"].execute($"UPDATE characters SET `group`=? WHERE identifier=?", new string[] { newgroup.ToString(), identifier });
        }

        public void setDead(bool dead)
        {
            isdead = dead;
            //int intdead = dead ? 1 : 0;
            SaveCharacter = true;
            //Exports["ghmattimysql"].execute("UPDATE characters SET `isdead` = ? WHERE `identifier` = ?", new object[] { intdead, identifier });
        }

        public void SaveCharacterInDb()
        {
            if (true)
            {
                Exports["ghmattimysql"].execute("UPDATE characters SET `group` = ?,`money` = ?,`gold` = ?,`rol` = ?,`xp` = ?,`job` = ?, `status` = ?,`firstname` = ?, `lastname` = ?, `jobgrade` = ?,`coords` = ?,`isdead` = ? WHERE `identifier` = ? AND `charidentifier` = ?", new object[] {group,money,gold,rol,xp,job,status,firstname,lastname,jobgrade,coords,isdead ? 1 : 0,identifier,charIdentifier });
                SaveCharacter = false;
            }
        }
    }
}
