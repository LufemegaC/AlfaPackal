﻿using Api_PACsServer.Modelos;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Models.Dto.Studies;

namespace Api_PACsServer.Services.IService.Pacs
{
    /// <summary>
    /// Service interface for handling Instance-related operations in the PACS system.
    /// </summary>
    public interface IInstanceService
    {
        
        /// <summary>
        /// Creates a new Instance entity.
        /// </summary>
        /// <param name="createDto">The data transfer object containing the details of the Instance to be created.</param>
        /// <returns>the created Instance entity.</returns>
        Task<Instance> Create(InstanceCreateDto createDto);

        ///// <summary>
        ///// Gets an Instance entity by its ID (internal).
        ///// </summary>
        ///// <param name="seriesNumber">The system ID of the Instance.</param>
        ///// <returns>the Instance entity.</returns>
        //Task<Instance> GetByIdComponents(int studyId, int seriesNumber, int instanceNumber);

        /// <summary>
        /// Gets an Instance entity by its SOPInstanceUID.
        /// </summary>
        /// <param name="SOPInstanceUID">The UID of the Instance to be retrieved.</param>
        /// <returns>the Instance entity.</returns>
        Task<Instance> GetByUID(string SOPInstanceUID);

        /// <summary>
        /// Checks if an Instance entity exists by its SOPInstanceUID.
        /// </summary>
        /// <param name="SOPInstanceUID">The UID of the Instance to be checked.</param>
        /// <returns>a boolean indicating whether the Instance exists.</returns>
        Task<bool> ExistsByUID(string SOPInstanceUID);

        /// <summary>
        /// Retrieves all Instance entities associated with a given Study ID and Series Number.
        /// </summary>
        /// <param name="seriesIntanceUID">The UID of the Serie with the Instances are to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of InstanceDto objects
        /// representing all Instances associated with the specified Series.
        /// </returns>
        Task<IEnumerable<InstanceDto>> GetAllBySerieUID(string seriesIntanceUID);

        /// <summary>
        /// Maps the provided DICOM metadata to an InstanceCreateDto object.
        /// </summary>
        /// <param name="metadata">The metadata containing the main entities and DICOM information required to create an Instance.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an InstanceCreateDto object
        /// mapped from the provided metadata, ready to be used for creating a new Instance entity.
        /// </returns>
        Task<InstanceCreateDto> MapToCreateDto(MainEntitiesCreateDto metadata);
    }
}