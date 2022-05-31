using System;
using System.Collections.Generic;
using UserCreator.Core.Constants;
using UserCreator.Core.Providers;

namespace UserCreator.Core
{
    /// <summary>
    /// Responsible for generate appropriate ID for each field type
    /// </summary>
    public class IdentityManager
    {
        /// <summary>
        /// Since the file structure is in format: {Id},{Field (column name)}, value
        /// Each field (unique by name, corresponding with an Table column) will have its own Id generator
        /// For now, beside DateOfBirth and Salary, we will consider other field will have a same Id generator
        /// </summary>
        private readonly Dictionary<string, IdentityProvider> _identityProviders =
            new Dictionary<string, IdentityProvider>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// The Id generator' strategy is quite depend on how the table structure
        /// so if we dynamically generate ID base on the type of field it can result in unwanted behavior
        /// for safe, we we need another column we must register it here
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public int GetNext(string fieldName)
        {
            if (fieldName.Equals(FieldConstants.DateOfBirth, StringComparison.InvariantCultureIgnoreCase)
                || fieldName.Equals(FieldConstants.Salary, StringComparison.InvariantCultureIgnoreCase))
            {
                    if (!_identityProviders.ContainsKey(fieldName))
                        _identityProviders.Add(fieldName, new IdentityProvider());

                    return _identityProviders[fieldName].GetNext();
                
            }

            if (!_identityProviders.ContainsKey(FieldConstants.DataField))
                _identityProviders.Add(FieldConstants.DataField, new IdentityProvider());

            return _identityProviders[FieldConstants.DataField].GetNext();
        }

        /// <summary>
        /// When resuming from a failed session, there is chance we will read an existing file
        /// hence we need to update the Identity state to latest value else it will cause duplicate Id
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        internal void UpdateCurrentState(string fieldName, int value)
        {
            if (_identityProviders.ContainsKey(fieldName))
            {
                _identityProviders[fieldName].SetId(value); 
            }
            else
            {
                var identityProvider = new IdentityProvider();
                identityProvider.SetId(value); 
                _identityProviders.Add(fieldName, identityProvider);
            }
        }
    }
}