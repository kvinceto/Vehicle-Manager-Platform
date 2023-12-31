﻿namespace Vmp.Common
{
    public static class EntityValidationConstants
    {
        public static class Owner
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
            public const string NameErrorMessage = "Name must be between 3 and 50 characters";

            public const int InfoMaxLength = 255;
            public const string InfoErrorMessage = "Info can not be more than 255 characters";
        }

        public static class Vehicle
        {
            public const int NumberMinLength = 4;
            public const int NumberMaxLength = 12;
            public const string NumberErrorMessage = "Number must be between 4 and 12 characters";


            public const int VinMinLength = 11;
            public const int VinMaxLength = 17;
            public const string VinErrorMessage = "Vin must be between 11 and 17 characters";

            public const int MakeMaxLength = 50;
            public const string MakeErrorMessage = "Make must be maximum 50 characters";

            public const int ModelMaxLength = 50;
            public const string ModelErrorMessage = "Model must be maximum 50 characters";


            public const int MileageMinValue = 0;
            public const int MileageMaxValue = 999999;
            public const string MileageErrorMessage = "Mileage must be between 0 and 999999";


            public const decimal FuelQuantityMinValue = 0.00M;
            public const string FuelErrorMessage = "Fuel can not be less than 0";

            public const decimal FuelCapacityMinValue = 1.00M;

            public const decimal FuelCostRateMinValue = 0.00M;
            public const string FuelCostRateErrorMessage = "Fuel rate can not be less than 0";


            public const int ImgUrlMaxLength = 2048;
        }

        public static class TaskModel
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
            public const string NameErrorMessage = "Name must be between 3 and 50 characters";

            public const int DescriptionMinLength = 1;
            public const int DescriptionMaxLength = 1000;
            public const string DescriptionErrorMessage = "Description must be between 1 and 1000 characters";
            public const string DeadlineErrorMessage = "Dedline must be a valid date in format dd/MM/yyyy";
        }

        public class DateCheck
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public class MileageCheck
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int ExpectedMileageMinValue = 1;
            public const int ExpectedMileageMaxValue = 999999;
        }

        public class Waybill
        {
            public const int StartMileageMinValue = 0;
            public const int StartMileageMaxValue = 999999;

            public const int ExpectedMileageMinValue = 1;
            public const int ExpectedMileageMaxValue = 999999;

            public const int InfoMaxLength = 255;

            public const int RouteTraveledMinLength = 1;
            public const int RouteTraveledMaxLength = 255;
        }

        public class CostCenter
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
            public const string NameErrorMessage = "Name must be between 3 and 50 characters";
        }
    }
}