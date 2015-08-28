using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace GeoLocalizationCL
{

    public class Trace : ITrace
    {
        #region Atributes

        int idError;
        string className;
        string methodName;
        string message;
        ILog logger;

        #endregion

        #region Properties

        public int IdError
        {
            get
            {
                return idError;
            }
            set
            {
                idError = value;
            }
        }

        public string ClassName
        {
            get
            {
                return className;
            }
            set
            {
                className = value;
            }
        }

        public string MethodName
        {
            get
            {
                return methodName;
            }
            set
            {
                methodName = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        #endregion

        #region Methods

        public Trace() { }
        public Trace(string pClassName, string pMethodName)
        {
            ClassName = pClassName;
            MethodName = pMethodName;
            logger = LogManager.GetLogger(pClassName);
        }

        public void Info(string pMessage)
        {
            logger.Info(pMessage);
        }

        public void Info(string pMessage, Dictionary<string, string> d)
        {
            //Send a JSON object
            //var items = new Dictionary<string, string>();
            //items.Add("idVideo", nIdVideo);
            logger.Info(d);

        }

        public void Error(string pMessage, Exception e)
        {
            logger.Error(pMessage, e);
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose()
        {
            Close();
        }
        public void Close()
        {
            // Do what's necessary to close the file
            System.GC.SuppressFinalize(this);
        }

        #endregion
    }
}
