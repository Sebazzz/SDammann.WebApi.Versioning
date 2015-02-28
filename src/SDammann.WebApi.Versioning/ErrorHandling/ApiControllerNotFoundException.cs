namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Web.Http.Routing;
    using Request;

    /// <summary>
    /// Defines the exception that is thrown when an api controller is not found
    /// </summary>
    [Serializable]
    public class ApiControllerNotFoundException : BaseApiException {
        private readonly ControllerIdentification _controllerIdentification;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected ApiControllerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._controllerIdentification = (ControllerIdentification) info.GetValue("_controllerIdentification", typeof (ControllerIdentification));
        }

        /// <summary>
        /// Gets the controller name used in the request. Note that this may be null.
        /// </summary>
        public ControllerIdentification ControllerIdentification {
            get { return this._controllerIdentification; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        /// <param name="controllerIdentification"></param>
        public ApiControllerNotFoundException(string message, ControllerIdentification controllerIdentification) : base(message) {
            this._controllerIdentification = controllerIdentification;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        public ApiControllerNotFoundException() {}

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);

            info.AddValue("_controllerIdentification", this._controllerIdentification);
        }

        /// <summary>
        /// Creates a default exception 
        /// </summary>
        /// <returns></returns>
        public static ApiControllerNotFoundException Create(ControllerIdentification controllerIdentification) {
            return new ApiControllerNotFoundException(String.Format(ExceptionResources.ApiControllerNotFound, controllerIdentification), controllerIdentification);
        }
    }


    /// <summary>
    /// Defines the exception that is thrown when an api controller is not found
    /// </summary>
    [Serializable]
    public class AmbigiousApiRequestException : BaseApiException {
        private readonly ControllerIdentification _controllerIdentification;
        private readonly IHttpRoute _route;
        private readonly IEnumerable<Type> _matchingTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected AmbigiousApiRequestException(SerializationInfo info, StreamingContext context) : base(info, context) {
            this._controllerIdentification = (ControllerIdentification) info.GetValue("_controllerIdentification", typeof (ControllerIdentification));
            this._matchingTypes = (IEnumerable<Type>) info.GetValue("_matchingTypes", typeof (List<Type>));
        }

        /// <summary>
        /// Gets the controller name used in the request. Note that this may be null.
        /// </summary>
        public ControllerIdentification ControllerIdentification {
            get { return this._controllerIdentification; }
        }

        /// <summary>
        /// Gets the selected route in the request
        /// </summary>
        public IHttpRoute Route {
            [DebuggerStepThrough] get { return this._route; }
        }

        /// <summary>
        /// Gets the matching types
        /// </summary>
        public IEnumerable<Type> MatchingTypes {
            [DebuggerStepThrough] get { return this._matchingTypes; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        public AmbigiousApiRequestException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        public AmbigiousApiRequestException(string message, ControllerIdentification controllerIdentification, IHttpRoute route, IEnumerable<Type> matchingTypes) : base(message) {
            this._controllerIdentification = controllerIdentification;
            this._route = route;
            this._matchingTypes = matchingTypes;
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);

            info.AddValue("_controllerIdentification", this._controllerIdentification);
            info.AddValue("_matchingTypes", this._matchingTypes.ToList());
        }

        /// <summary>
        /// Creates a default exception 
        /// </summary>
        /// <returns></returns>
        public static AmbigiousApiRequestException Create(ControllerIdentification controllerIdentification, IHttpRoute route, IEnumerable<Type> matchingTypes) {
            Contract.Assert(route != null);
            Contract.Assert(controllerIdentification != null);
            Contract.Assert(matchingTypes != null);

            // Generate an exception containing all the controller types
            StringBuilder typeList = new StringBuilder();
            foreach (Type matchedType in matchingTypes)
            {
                typeList.AppendLine();
                typeList.Append(matchedType.FullName);
            }

            return new AmbigiousApiRequestException(
                String.Format(ExceptionResources.AmbigiousControllerRequest, controllerIdentification, route.RouteTemplate, typeList), 
                controllerIdentification,
                route,
                matchingTypes);
        }
    }

    /// <summary>
    /// Represents the base class for all exceptions thrown in the API versioning system
    /// </summary>
    [Serializable]
    public abstract class BaseApiException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        protected BaseApiException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        protected BaseApiException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param><param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
        protected BaseApiException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected BaseApiException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }


    /// <summary>
    /// Defines the exception thrown when the api version in the request cannot be determined, e.g. when <see cref="IRequestControllerIdentificationDetector"/> returns <c>null</c>
    /// </summary>
    [Serializable]
    public class ApiVersionNotDeterminedException : BaseApiException {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        public ApiVersionNotDeterminedException() : this(ExceptionResources.CannotDetermineRequestVersion) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public ApiVersionNotDeterminedException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param><param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
        public ApiVersionNotDeterminedException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        public ApiVersionNotDeterminedException(SerializationInfo info, StreamingContext context) : base(info, context) {}

        /// <summary>
        /// Creates an instance of this exception with the default message
        /// </summary>
        /// <returns></returns>
        public static ApiVersionNotDeterminedException Create() {
            return new ApiVersionNotDeterminedException();
        }
    }
}