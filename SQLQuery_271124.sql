
-- Pruebas de consultas QIDO-RS

-- "?includefield=00201206&includefield=00201208&00080020=%3E20230101&00100010=perez*"

SELECT s.StudyInstanceUID, s.StudyDate, s.StudyTime, sd.AccessionNumber,
s.PatientName, s.PatientID, s.StudyDescription, sd.NumberOfStudyRelatedSeries,
sd.NumberOfStudyRelatedInstances, s.ReferringPhysicianName, 
(SELECT STRING_AGG(sm.Modality, ', ') 
FROM StudyModalities AS sm 
WHERE sm.StudyInstanceUID = s.StudyInstanceUID) AS ModalitiesInStudy, 
NumberOfStudyRelatedSeries, NumberOfStudyRelatedInstances 
FROM Studies AS s LEFT JOIN StudyDetails AS sd ON s.StudyInstanceUID = sd.StudyInstanceUID 
WHERE 1 = 1  AND StudyDate > '2023-01-01' AND REPLACE(PatientName, '^', '') LIKE '%PEREZ%' 
ORDER BY s.StudyDate DESC


-- "?includefield=00201206,00201208&00080020=%3E20230101&00100010=perez*"

SELECT s.StudyInstanceUID, s.StudyDate, s.StudyTime, sd.AccessionNumber, s.PatientName, 
s.PatientID, s.StudyDescription, sd.NumberOfStudyRelatedSeries, 
sd.NumberOfStudyRelatedInstances, s.ReferringPhysicianName, 
(SELECT STRING_AGG(sm.Modality, ', ') 
FROM StudyModalities AS sm 
WHERE sm.StudyInstanceUID = s.StudyInstanceUID) AS ModalitiesInStudy, 
NumberOfStudyRelatedSeries, NumberOfStudyRelatedInstances 
FROM Studies AS s 
LEFT JOIN StudyDetails AS sd ON s.StudyInstanceUID = sd.StudyInstanceUID 
WHERE 1 = 1  AND StudyDate > '2023-01-01' AND REPLACE(PatientName, '^', '') 
LIKE '%PEREZ%'  ORDER BY s.StudyDate DESC


-- $"?patientname=Al*&limit={limit}&page={pageNumber}&pagesize={pageSize}"
-- $"?patientname=Al*&limit={25}&page={1}&pagesize={10}"

-- NOTA, NECESITO PROBARLO CON MAS VALORES.

SELECT s.StudyInstanceUID, s.StudyDate, s.StudyTime, sd.AccessionNumber, s.PatientName, 
s.PatientID, s.StudyDescription, sd.NumberOfStudyRelatedSeries, sd.NumberOfStudyRelatedInstances, 
s.ReferringPhysicianName, 
(SELECT STRING_AGG(sm.Modality, ', ') FROM StudyModalities AS sm 
WHERE sm.StudyInstanceUID = s.StudyInstanceUID) AS ModalitiesInStudy 
FROM Studies AS s 
LEFT JOIN StudyDetails AS sd ON s.StudyInstanceUID = sd.StudyInstanceUID
WHERE 1 = 1  AND REPLACE(PatientName, '^', '') LIKE '%AL%' ORDER BY s.StudyDate 
DESC OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY


-- $"?patientname=Al*&StudyDate=20231001-20231231"



SELECT s.StudyInstanceUID, s.StudyDate, s.StudyTime, sd.AccessionNumber, s.PatientName, s.PatientID, s.StudyDescription, 
sd.NumberOfStudyRelatedSeries, sd.NumberOfStudyRelatedInstances, s.ReferringPhysicianName, 
(SELECT STRING_AGG(sm.Modality, ', ') FROM StudyModalities AS sm WHERE sm.StudyInstanceUID = s.StudyInstanceUID) AS ModalitiesInStudy 
FROM Studies AS s LEFT JOIN StudyDetails AS sd ON s.StudyInstanceUID = sd.StudyInstanceUID 
WHERE 1 = 1  AND StudyDate BETWEEN '2023-10-01' AND '2023-12-31'
AND REPLACE(PatientName, '^', '') LIKE '%AL%' ORDER BY s.StudyDate DESC

