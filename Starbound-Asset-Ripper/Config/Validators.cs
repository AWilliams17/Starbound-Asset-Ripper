using Registrar;
using System.IO;

namespace Starbound_Asset_Ripper.Config
{
    class Validators
    {
        class DirectoryValidatorClass : IValidator
        {
            public string Description()
            {
                return "Option must be a path to a directory which exists.";
            }

            public bool Validate(object value)
            {
                return Directory.Exists(ValidatorConverters.ValidatorStringConverter(value));
            }
        }

        public IValidator DirectoryValidator = new DirectoryValidatorClass();
    }
}
