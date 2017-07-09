using System.ComponentModel;

namespace L2dotNET.Network.serverpackets
{
    public enum CharCreateFailReason
    {
        [Description("Your character creation has failed.")]
        CreationFailed = 0,
        [Description("You cannot create another character. Please delete the existing character and try again.")]
        TooManyCharsOnAccount = 1,
        [Description("This name already exists.")]
        NameAlreadyExists = 2,
        [Description("Names must be between 1-16 characters, excluding spaces or special characters.")]
        InvalidNamePattern = 3,
        [Description("Incorrect name. Please try again.")]
        IncorrectName = 4,
        [Description("Characters cannot be created from this server.")]
        CharCreationBlocked = 5
    }
}