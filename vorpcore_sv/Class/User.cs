using System;
using System.Collections.Generic;
using System.Data.Common;
using CitizenFX.Core;

namespace vorpcore_sv.Class
{
    //class for users that contains their characters
    public class User:BaseScript
    {
        private string _identifier; //User steamid    
        private string _group;//User admin group
        private int _playerwarnings;//Used for admins to know how many warnings a user has
        private Dictionary<int,Character> _usercharacters;
        private int _numofcharacters;
        private int usedCharacterId;

        public int UsedCharacterId
        {
            get => usedCharacterId;
            set => usedCharacterId = value;
        }

        public int Numofcharacters
        {
            get => _numofcharacters;
            set => _numofcharacters = value;
        }

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
                Exports["ghmattimysql"].execute("UPDATE users SET 'group' = ? WHERE identifier=?", new object[] { Group, Identifier });
            }
        }

        public int Playerwarnings
        {
            get => _playerwarnings;
            set
            {
                _playerwarnings = value;
                Exports["ghmattimysql"].execute("UPDATE users SET warnings= ? WHERE identifier=?", new object[] { Playerwarnings, Identifier });
            }
        }
        

        public User(string identifier, string group, int playerwarnings)
        {
            _identifier = identifier;
            _group = group;
            _playerwarnings = playerwarnings;
            _usercharacters = new Dictionary<int, Character>();
            LoadCharacters(identifier);
            //Cargarmos todos sus characters de la base de datos si al cargarlos no tiene entonces cuando se llame a spawnpalyer habrá que crear 1
        }

        private async void LoadCharacters(string identifier)
        {
            Debug.WriteLine("Usuario "+identifier+" cargado");
            List<object> usercharacters = await Exports["ghmattimysql"].executeSync("SELECT * FROM characters WHERE identifier =?", new[] {identifier});
            Numofcharacters = usercharacters.Count;
            if (Numofcharacters > 0)
            {
                //Metemos todos los characters en el diccionario
                foreach (object icharacter in usercharacters)
                {
                    IDictionary<string, object> character = (dynamic)icharacter;
                    Debug.WriteLine((string)character["identifier"]);
                    Character newCharacter = new Character((string)character["identifier"],(int)character["charidentifier"],(string)character["group"],
                        (string)character["job"],(string)character["jobgrade"],(string)character["firstname"],(string)character["lastname"],(string)character["inventory"],
                        (string)character["status"],(string)character["coords"],(double)character["money"],(double)character["gold"],(double)character["rol"],(int)character["xp"],
                        (bool) character["isdead"]);
                    if (_usercharacters.ContainsKey(newCharacter.CharIdentifier))
                    {
                        _usercharacters[newCharacter.CharIdentifier] = newCharacter;
                    }
                    else
                    {
                        _usercharacters.Add(newCharacter.CharIdentifier,newCharacter);
                    }
                }
            }
            else
            {
                //Le decimos que hay que crear un nuevo character
            }
            Debug.WriteLine($"El jugador tiene {usercharacters.Count}");
        }

        public void addCharacter(Character newCharacter)
        {
            if (_usercharacters.ContainsKey(newCharacter.CharIdentifier)) return;
            _usercharacters.Add(newCharacter.CharIdentifier,newCharacter);
        }

        public void delCharacter(int charIdentifier)
        {
            if (_usercharacters.ContainsKey(charIdentifier))
            {
                _usercharacters.Remove(charIdentifier);
            }
        }

        public Character GetUsedCharacter()
        {
            if (_usercharacters.ContainsKey(UsedCharacterId))//Comprobante para asegurarnos de que existe aunque lo demos por hecho
            {
                return _usercharacters[UsedCharacterId];
            }
            else
            {
                return null;
            }
        }
    }
}