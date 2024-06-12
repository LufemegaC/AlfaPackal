namespace Utileria
{
    public class DicomValues
    {
        public enum Modality
        {
            cr, // Radiografio computarizada
            CT, // Tomografía Computarizada
            MR, // Resonancia Magnética
            US, // Ultrasonido
            XR, // Radiografía
            NM, // Medicina Nuclear
            DX, // Radiografía Digital
            MG, // Mamografía
            IO, // Imagen Intraoral
            OT  // Otro
        }

        public enum PatientSex
        {
            O, // Representa un valor no especificado o desconocido
            U,
            M, // Masculino
            F // Femenino
        }

        public enum PatientPosition
        {
            HFP,//(Head First-Prone): Cabeza primero en posición prona.
            HFS,//(Head First-Supine): Cabeza primero en posición supina.
            HFDR,//(Head First-Decubitus Right): Cabeza primero en decúbito derecho.
            HFDL,//(Head First-Decubitus Left): Cabeza primero en decúbito izquierdo.
            FFP,//(Feet First-Prone): Pies primero en posición prona.
            FFS,//(Feet First-Supine): Pies primero en posición supina.
            FFDR,//(Feet First-Decubitus Right): Pies primero en decúbito derecho.
            FFDL//(Feet First-Decubitus Left): Pies primero en decúbito izquierdo.
        }
    
        public enum PhotometricInterpretation
        //Interpretación de los datos de color
        {
            MONOCHROME1,// Imagen en escala de grises donde los valores más altos representan tonos más claros.
            MONOCHROME2,//Imagen en escala de grises donde los valores más altos representan tonos más oscuros.
            RGB,//Imagen en color con canales para rojo, verde y azul.
            YBR_FULL,//Imagen en color utilizando la codificación YCbCr.
        }

        public enum BodyPartExamined
        {
            HEAD, // Cabeza
            NECK, // Cuello
            CHEST, // Tórax
            ABDOMEN, // Abdomen
            PELVIS, // Pelvis
            SPINE, // Columna vertebral
            SHOULDER, // Hombro
            ELBOW, // Codo
            WRIST, // Muñeca
            HAND, // Mano
            HIP, // Cadera
            KNEE, // Rodilla
            ANKLE, // Tobillo
            FOOT, // Pie
            HEART, // Corazón
            LUNG, // Pulmón
            LIVER, // Hígado
            KIDNEY, // Riñón
            BRAIN // Cerebro
        }


    }
}