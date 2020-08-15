using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace vorpcore_sv.Class
{
    //class for users that contains their characters
    public class User:BaseScript
    {
        private string _identifier; //User steamid    
        private string _group;//User admin group
        private int _playerwarnings;//Used for admins to know how many warnings a user has
        private bool _banned;//Used for know if a player is banned
        private Dictionary<string,Character> _usercharacters;

        public string Identifier
        {
            get => _identifier;
            set
            {
                Exports["ghmattimysql"].execute("UPDATE users SET identifier= ? WHERE identifier=?", new object[] { Identifier, value });
                _identifier = value;
            }
        }

        public string Group
        {
            get => _group;
            set
            {
                _group = value;
                Exports["ghmattimysql"].execute("UPDATE users SET group= ? WHERE identifier=?", new object[] { Group, Identifier });
            }
        }

        public int Playerwarnings
        {
            get => _playerwarnings;
            set
            {
                _playerwarnings = value;
                Exports["ghmattimysql"].execute("UPDATE users SET playerwarnings= ? WHERE identifier=?", new object[] { Playerwarnings, Identifier });
            }
        }

        public bool Banned
        {
            get => _banned;
            set
            {
                _banned = value;
                Exports["ghmattimysql"].execute("UPDATE users SET banned= ? WHERE identifier=?", new object[] { Banned, Identifier });
            }
        }

        public User(string identifier, string group, int playerwarnings, bool banned)
        {
            Identifier = identifier;
            Group = group;
            Playerwarnings = playerwarnings;
            Banned = banned;
            _usercharacters = new Dictionary<string, Character>();
        }

        public void addCharacter(Character newCharacter)
        {
            if (_usercharacters.ContainsKey(newCharacter.Identifier)) return;
            _usercharacters.Add(newCharacter.Identifier,newCharacter);
        }

        public void delCharacter(string charIdentifier)
        {
            if (_usercharacters.ContainsKey(charIdentifier))
            {
                _usercharacters.Remove(charIdentifier);
            }
        }
    }
}