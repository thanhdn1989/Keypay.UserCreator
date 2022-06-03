using System;
using System.Collections.Generic;
using System.IO;
using UserCreator.Core.Constants;

namespace UserCreator.Core
{
    /// <summary>
    /// Recover last failed session if needed
    /// </summary>
    public class RecoveryService
    {
        private readonly IdentityManager _identityManager;

        public RecoveryService(IdentityManager identityManager)
        {
            _identityManager = identityManager;
        }

        public void TryRecoverLastSession(string path)
        {
            /*
             * We need to find the latest ID of each field type and update it to IdentityProvider so we won't have
             * duplicate ID when continue working on existing file
             */
            if (!File.Exists(path)) return;
            var fieldDict = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var line in File.ReadLines(path))
            {
                var fields = line.Split(",");
                if (fields.Length != 3)
                    throw new Exception("Invalid data");
                var fieldName = fields[1].Trim();
                var fieldValue = int.Parse(fields[0]);
                if (fieldName.Equals(FieldConstants.DateOfBirth, StringComparison.InvariantCultureIgnoreCase)
                    || fieldName.Equals(FieldConstants.Salary, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!fieldDict.ContainsKey(fieldName))
                    {
                        fieldDict.Add(fieldName, fieldValue);
                        continue;
                    }

                    fieldDict[fieldName] = Math.Max(fieldValue, fieldDict[fieldName]);
                }
                else
                {
                    if (!fieldDict.ContainsKey(FieldConstants.DataField))
                    {
                        fieldDict.Add(FieldConstants.DataField, fieldValue);
                        continue;
                    }

                    fieldDict[FieldConstants.DataField] = Math.Max(fieldDict[FieldConstants.DataField], fieldValue);
                }
            }

            foreach (var kvp in fieldDict)
            {
                _identityManager.UpdateCurrentState(kvp.Key, kvp.Value);
            }
            
            Console.WriteLine($"Found existing file at {path}, Finish recover last session");
        }
    }
}