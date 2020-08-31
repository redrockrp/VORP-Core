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
        private int usedCharacterId = -1;

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
        }

        public string Group
        {
            get => _group;
            set
            {
                _group = value;
                Exports["ghmattimysql"].execute("UPDATE users SET `group` = ? WHERE `identifier` = ?", new object[] { _group, Identifier });
                Debug.WriteLine("changedGroup");
            }
        }

        public int Playerwarnings
        {
            get => _playerwarnings;
            set
            {
                _playerwarnings = value;
                Exports["ghmattimysql"].execute("UPDATE users SET `warnings` = ? WHERE `identifier` = ?", new object[] { _playerwarnings, Identifier });
                Debug.WriteLine("playerwarnings");
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

        public Dictionary<string, dynamic> GetUser()
        {
            Dictionary<string,dynamic> character = new Dictionary<string, dynamic>();
            if (_usercharacters.ContainsKey(usedCharacterId))
            {
                character = _usercharacters[usedCharacterId].getCharacter();
            }
            List<Dictionary<string,dynamic>> userCharacters = new List<Dictionary<string,dynamic>>();
            foreach (KeyValuePair<int,Character> chara in _usercharacters)
            {
                userCharacters.Add(chara.Value.getCharacter());
            }
            Dictionary<string,dynamic> auxdic = new Dictionary<string, dynamic>
            {
                ["getIdentifier"] = Identifier,
                ["getGroup"] = Group,
                ["getPlayerwarnings"] = Playerwarnings,
                ["setGroup"] = new Action<string>((group) =>
                {
                    Group = group;
                }),
                ["setPlayerWarnings"] = new Action<int>((warnings) =>
                {
                    Playerwarnings = warnings;
                }),
                ["getUsedCharacter"] = character,
                ["getUserCharacters"] = userCharacters,
                ["getNumOfCharacters"] = _numofcharacters
            };
            return auxdic;
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
                    if (character.ContainsKey("identifier"))
                    {
                        Character newCharacter = new Character(identifier,(int) character["charidentifier"],(string)character["group"],
                            (string) character["job"],int.Parse(character["jobgrade"].ToString()),(string) character["firstname"],(string) character["lastname"]
                            ,(string) character["inventory"],
                            (string) character["status"],(string) character["coords"],double.Parse(character["money"].ToString())
                            ,double.Parse(character["gold"].ToString()),double.Parse(character["rol"].ToString()),int.Parse(character["xp"].ToString()), (bool)character["isdead"],(string)character["skinPlayer"],
                            (string)character["compPlayer"]);
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

        public void SaveUser()
        {
            foreach (KeyValuePair<int,Character> character in _usercharacters)
            {
                character.Value.SaveCharacterInDb();
            }
        }
    }
}