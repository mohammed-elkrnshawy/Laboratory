using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical
{
    class Patient_Normal_Class
    {
        private string analysisUnit;
        private string patientNormal;
        private int patientAge;
        private int patientPeriod;
        private bool patientGender;

        public void set_patientAge(int patient_age)
        {
            patientAge = patient_age;
        }
        
        public void set_analysisUnit(string analysis_unit)
        {
            analysisUnit = analysis_unit;
        }

        public void set_patientNormal(string patient_normal)
        {
            patientNormal = patient_normal;
        }
        public void set_patientPeriod(int patient_period)
        {
            patientPeriod = patient_period;
        }

        public void set_patientGender(bool patient_gender)
        {
            patientGender = patient_gender;
        }

        public int get_PatientAge()
        {
            return patientAge;
        }
        
        public int get_PatientPeriod()
        {
            return patientPeriod;
        }
        
        public bool get_PatientGender()
        {
            return patientGender;
        }
        
        public string get_PatientNormal()
        {
            return patientNormal;
        }
        
        public string get_analysisUnit()
        {
            return analysisUnit;
        }
    }
}
