using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using FellowOakDicom;
using FellowOakDicom.Network;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace Api_PACsServer.Models.Dto.Base
{
    /// <summary>
    /// Base class representing common DICOM query parameters for different levels in DICOM QIDO-RS queries.
    /// This abstract class is designed to be inherited by specific DTOs representing Study, Series, and Instance level queries.
    /// It contains shared properties that are commonly used in DICOM queries across multiple levels.
    /// </summary>
    public abstract class BaseDicomQueryParametersDto
    {
        /// <summary>
        /// List of fields to add in base select.
        /// </summary>
        public List<string?> IncludeFields { get; set; } = new List<string?>();

        /// <summary>
        /// Unique identifier for a Study. This parameter is relevant in Study, Series, and Instance level queries.
        /// </summary>
        public QueryParameter? StudyInstanceUID { get; set; }

        /// <summary>
        /// Unique identifier for a Series. This parameter is relevant in Series and Instance level queries.
        /// </summary>
        public QueryParameter? SeriesInstanceUID { get; set; }

        /// <summary>
        /// Unique identifier for an Instance (SOP Instance). This parameter is relevant for Instance level queries.
        /// </summary>
        public QueryParameter? SOPInstanceUID { get; set; }

        /// <summary>
        /// Represents the modality of the imaging equipment. This parameter is typically used in Series and Instance level queries.
        /// </summary>
        public QueryParameter? Modality { get; set; }

        /// <summary>
        /// Indicates the availability status of an instance, which can be used across different query levels to determine if data is online or archived.
        /// </summary>
        public QueryParameter? InstanceAvailability { get; set; }

        /// <summary>
        /// Defines the DICOM query-retrieve level for filtering data.
        /// </summary>
        public DicomQueryRetrieveLevel DicomQueryLevel { get; set; }
        
        /// <summary>
        /// Accession number assigned by the hospital information system, which can be used across Study, Series, and Instance level queries.
        /// </summary>
        public QueryParameter? AccessionNumber { get; set; }

        /// <summary>
        /// Property to be overridden by derived classes to specify the entity type (e.g., typeof(Study), typeof(Series)).
        /// </summary>
        protected abstract Type EntityType { get; }

        public void Validateparameters() 
        {
            ValidateBaseParameters();
            ValidateDerivedParameters();
        }

        /// <summary>
        /// Validaciones comunes a todos los niveles. Por ejemplo, verificar el formato de los UIDs si existen.
        /// </summary>
        protected virtual void ValidateBaseParameters() 
        {
            // Validate UIDs structure
            if (string.IsNullOrEmpty(StudyInstanceUID.Value) && !DicomUID.IsValidUid(StudyInstanceUID.Value))
                throw new ArgumentException($"The Study UID value {StudyInstanceUID.Value}' is not in a valid UID format.");
            if (string.IsNullOrEmpty(SeriesInstanceUID.Value) && !DicomUID.IsValidUid(SeriesInstanceUID.Value))
                throw new ArgumentException($"The Serie UID provided {SeriesInstanceUID.Value}' is not in a valid UID format.");
            if (string.IsNullOrEmpty(SOPInstanceUID.Value) && !DicomUID.IsValidUid(SOPInstanceUID.Value))
                throw new ArgumentException($"The Instance UID provided {SOPInstanceUID.Value}' is not in a valid UID format.");
            //Validate includefields and main entity
            ValidateIncludeFields();
        }

        /// <summary>
        /// Validates the IncludeFields property to ensure all fields exist as properties in the class T.
        /// </summary>
        /// <typeparam name="T">The type representing the database entity.</typeparam>
        /// <exception cref="ArgumentException">Thrown if an invalid field is found in IncludeFields.</exception>
        protected void ValidateIncludeFields()
        {
            if (IncludeFields == null || !IncludeFields.Any())
            {
                return; // No fields to validate
            }

            // Get the list of property names from the EntityType
            var validProperties = EntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Select(p => p.Name)
                                            .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Check if any field in IncludeFields does not exist in the properties of the entity type
            var invalidFields = IncludeFields.Where(field => !validProperties.Contains(field)).ToList();

            if (invalidFields.Any())
            {
                // Throw an exception listing all invalid fields
                throw new ArgumentException($"The following fields are not valid for type {EntityType.Name}: {string.Join(", ", invalidFields)}");
            }
        }

        protected abstract void ValidateDerivedParameters();

        

    }
}
