﻿using Syfuhs.Security.Kerberos.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syfuhs.Security.Kerberos.Entities
{
    public class NegTokenInit : Asn1ValueType
    {
        public NegTokenInit(Asn1Element sequence)
        {
            Asn1Value = sequence.Value;

            for (var i = 0; i < sequence.Count; i++)
            {
                var element = sequence[i];

                switch (element.ContextSpecificTag)
                {
                    case 0:
                        SetMechTypes(element);
                        break;

                    case 2:
                        MechToken = new InitialContextToken(element);
                        break;
                }
            }
        }

        private void SetMechTypes(Asn1Element sequence)
        {
            for (var i = 0; i < sequence.Count; i++)
            {
                var element = sequence[i];

                for (var j = 0; j < element.Count; j++)
                {
                    var childNode = element[i];

                    if (childNode.ContextSpecificTag == MechType.ContextTag)
                    {
                        MechTypes.Add(new MechType(childNode.AsString()));
                    }
                }
            }

            if (MechTypes.Any(m => m.Oid == MechType.NTLM))
            {
                throw new NotSupportedException("NTLM is not supported");
            }
        }

        private List<MechType> mechTypes;

        public List<MechType> MechTypes { get { return mechTypes ?? (mechTypes = new List<MechType>()); } }

        public InitialContextToken MechToken { get; private set; }
    }
}
