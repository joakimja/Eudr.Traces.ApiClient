using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eudr.Traces.Integrations.Authentications
{
    public class XmlElementMessageHeader : MessageHeader
    {
        private readonly XmlElement _element;

        public XmlElementMessageHeader(XmlElement element)
        {
            _element = element;
        }

        public override string Name => _element.LocalName;
        public override string Namespace => _element.NamespaceURI;
        public override bool MustUnderstand => true;

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            _element.WriteContentTo(writer);
        }
    }

}
