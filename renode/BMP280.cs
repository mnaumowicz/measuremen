//
// Copyright (c) 2010-2020 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Linq;
using System.Collections.Generic;
using Antmicro.Renode.Peripherals.Sensor;
using Antmicro.Renode.Peripherals.I2C;
using Antmicro.Renode.Logging;
using Antmicro.Renode.Core;
using Antmicro.Renode.Core.Structure.Registers;
using Antmicro.Renode.Utilities;

namespace Antmicro.Renode.Peripherals.Sensors
{
    public class BMP280 : II2CPeripheral, IProvidesRegisterCollection<ByteRegisterCollection>, ITemperatureSensor
    {
        public BMP280()
        {
            RegistersCollection = new ByteRegisterCollection(this);
            DefineRegisters();
        }

        public void Reset()
        {
            RegistersCollection.Reset();
            registerAddress = 0;
            this.Log(LogLevel.Noisy, "Reset registers");
        }

        public void Write(byte[] data)
        {
            if(data.Length == 0)
            {
                this.Log(LogLevel.Warning, "Unexpected write with no data");
                return;
            }

            this.Log(LogLevel.Noisy, "Write with {0} bytes of data: {1}", data.Length, Misc.PrettyPrintCollectionHex(data));
            registerAddress = (Registers)data[0];

            if(data.Length > 1)
            {
                // skip the first byte as it contains register address
                foreach(var b in data.Skip(1))
                {
                    this.Log(LogLevel.Noisy, "Writing 0x{0:X} to register {1} (0x{1:X})", b, registerAddress);
                    RegistersCollection.Write((byte)registerAddress, b);
                }
            }
            else
            {
                this.Log(LogLevel.Noisy, "Preparing to read register {0} (0x{0:X})", registerAddress);
            }
        }

        public byte[] Read(int count)
        {
            this.Log(LogLevel.Noisy, "Reading {0} bytes from register {1} (0x{1:X})", count, registerAddress);
            var result = new byte[count];
            for(var i = 0; i < result.Length; i++)
            {
                result[i] = RegistersCollection.Read((byte)registerAddress);
                this.Log(LogLevel.Noisy, "Read value {0} from register {1} (0x{1:X})", result[i], registerAddress);
                RegistersAutoIncrement();
            }
            return result;
        }

        public void FinishTransmission()
        {
        }

        public decimal Temperature
        {
            get => temperature;
            set
            {
                if(value < MinTemperature | value > MaxTemperature)
                {
                    this.Log(LogLevel.Warning, "Temperature is out of range. Supported range: {0} - {1}", MinTemperature, MaxTemperature);
                }
                else
                {
                    temperature = value;
                    this.Log(LogLevel.Noisy, "Sensor temperature set to {0}", temperature);
                }
            }
        }

        public int UncompensatedPressure { get; set; }

        public ByteRegisterCollection RegistersCollection { get; }

        private void DefineRegisters()
        {
            Registers.CoefficientCalibration88.Define(this, 0x70)
		.WithValueField(0, 8, out coeffCalib88, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration89.Define(this, 0x6B)
		.WithValueField(0, 8, out coeffCalib89, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration8A.Define(this, 0x43)
		.WithValueField(0, 8, out coeffCalib8A, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration8B.Define(this, 0x67)
		.WithValueField(0, 8, out coeffCalib8B, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration8C.Define(this, 0x18)
		.WithValueField(0, 8, out coeffCalib8C, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration8D.Define(this, 0xFC)
		.WithValueField(0, 8, out coeffCalib8D, FieldMode.Read, name: "AC5[15-8]");


            Registers.CoefficientCalibration8E.Define(this, 0x7D)
		.WithValueField(0, 8, out coeffCalib8E, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration8F.Define(this, 0x8E)
		.WithValueField(0, 8, out coeffCalib8F, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration90.Define(this, 0x43)
		.WithValueField(0, 8, out coeffCalib90, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration91.Define(this, 0xD6)
		.WithValueField(0, 8, out coeffCalib91, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration92.Define(this, 0xD0)
		.WithValueField(0, 8, out coeffCalib92, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration93.Define(this, 0x0B)
		.WithValueField(0, 8, out coeffCalib93, FieldMode.Read, name: "AC5[15-8]");

            Registers.CoefficientCalibration94.Define(this, 0x27)
		.WithValueField(0, 8, out coeffCalib94, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration95.Define(this, 0x0B)
		.WithValueField(0, 8, out coeffCalib95, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration96.Define(this, 0x8C)
		.WithValueField(0, 8, out coeffCalib96, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration97.Define(this, 0x00)
		.WithValueField(0, 8, out coeffCalib97, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration98.Define(this, 0xF9)
		.WithValueField(0, 8, out coeffCalib98, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration99.Define(this, 0xFF)
		.WithValueField(0, 8, out coeffCalib99, FieldMode.Read, name: "AC5[15-8]");

            Registers.CoefficientCalibration9A.Define(this, 0x8C)
		.WithValueField(0, 8, out coeffCalib9A, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration9B.Define(this, 0x3C)
		.WithValueField(0, 8, out coeffCalib9B, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration9C.Define(this, 0xF8)
		.WithValueField(0, 8, out coeffCalib9C, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration9D.Define(this, 0xC6)
		.WithValueField(0, 8, out coeffCalib9D, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration9E.Define(this, 0x70)
		.WithValueField(0, 8, out coeffCalib9E, FieldMode.Read, name: "AC5[15-8]");
            Registers.CoefficientCalibration9F.Define(this, 0x17)
		.WithValueField(0, 8, out coeffCalib9F, FieldMode.Read, name: "AC5[15-8]");

            Registers.ChipID.Define(this, 0x58); //RO

            Registers.SoftReset.Define(this, 0x0) //WO
                .WithWriteCallback((_, val) =>
                {
                    if(val == resetCommand)
                    {
                        Reset();
                    }
                });

            Registers.CtrlMeasurement.Define(this, 0x0) //RW
                .WithValueField(0, 1, out ctrlMeasurement , name: "CTRL_MEAS")
                .WithFlag(1, out startConversion, name: "SCO")
                .WithValueField(4, 2, out controlOversampling, name: "OSS")
                .WithWriteCallback((_, __) => HandleMeasurement());

            Registers.OutMSB.Define(this, 0x80)
                .WithValueField(0, 8, out outMSB, FieldMode.Read, name: "OUT_MSB")
                .WithReadCallback((_, __) => HandleMeasurement());

            Registers.OutLSB.Define(this, 0x00)
                .WithValueField(0, 8, out outLSB, FieldMode.Read, name: "OUT_LSB");

            Registers.OutXLSB.Define(this, 0x00)
                .WithValueField(0, 8, out outXLSB, FieldMode.Read, name: "OUT_XLSB");

            Registers.press_msb.Define(this, 0x80)
                .WithValueField(0, 8, out press_outMSB, FieldMode.Read, name: "PRESS_MSB")
                .WithReadCallback((_, __) => HandleMeasurement());

            Registers.press_lsb.Define(this, 0x00)
                .WithValueField(0, 8, out press_outLSB, FieldMode.Read, name: "PRESS_LSB");

            Registers.press_xlsb.Define(this, 0x00)
                .WithValueField(0, 8, out press_outXLSB, FieldMode.Read, name: "PRESS_XLSB");

	    Registers.Status.Define(this, 0x0)
		.WithReadCallback((_, __) => HandleMeasurement());
        }

        private void RegistersAutoIncrement()
        {
            if((registerAddress >= Registers.CoefficientCalibration88 &&
                registerAddress < Registers.CoefficientCalibration9F) ||
               (registerAddress >= Registers.press_msb && registerAddress < Registers.OutXLSB))
            {
                registerAddress = (Registers)((int)registerAddress + 1);
                this.Log(LogLevel.Noisy, "Auto-incrementing to the next register 0x{0:X} - {0}", registerAddress);
            }
        }

        private int GetUncompensatedTemperature()
        {
		ushort T1=(ushort)((coeffCalib89.Value << 8) + coeffCalib88.Value);
		short T2=(short)((coeffCalib8B.Value << 8) + coeffCalib8A.Value);
		short T3=(short)((coeffCalib8D.Value << 8) + coeffCalib8C.Value);
		double T= (16.0 *(5.0 *(double)((int)T1) *(double)((int)T3) + 32768.0* Math.Sqrt(5.0) *Math.Sqrt(5.0 *(double)((int)T2)*(double)((int)T2) + 16.0* (double)((int)T3) *(double)((int)(temperature*100)) - 8.0* (float)((int)T3)) - 163840.0* (double)((int)T2)))/(5.0* (double)((int)T3));
		int var1, var2, TT;
		TT=(int)T;
		var1 = ((((TT>>3) - ((int)T1<<1))) * ((int)T2)) >> 11;
		var2 = (((((TT>>4) - ((int)T1)) * ((TT>>4) - ((int)T1)))>> 12) *((int)T3)) >> 14;
		t_fine = var1 + var2;
		return (int)T;
        }

	uint BME280_compensate_P_int64(int adc_P)
	{
		ushort P1=(ushort)((coeffCalib8F.Value << 8) + coeffCalib8E.Value);
		short P2=(short)((coeffCalib91.Value << 8) + coeffCalib90.Value);
		short P3=(short)((coeffCalib93.Value << 8) + coeffCalib92.Value);
		short P4=(short)((coeffCalib95.Value << 8) + coeffCalib94.Value);
		short P5=(short)((coeffCalib97.Value << 8) + coeffCalib96.Value);
		short P6=(short)((coeffCalib99.Value << 8) + coeffCalib98.Value);
		short P7=(short)((coeffCalib9B.Value << 8) + coeffCalib9A.Value);
		short P8=(short)((coeffCalib9D.Value << 8) + coeffCalib9C.Value);
		short P9=(short)((coeffCalib9F.Value << 8) + coeffCalib9E.Value);
		long var1, var2, p;
		var1 = ((long)t_fine) - 128000;
		var2 = var1 * var1 * (long)P6;
		var2 = var2 + ((var1*(long)P5)<<17);
		var2 = var2 + (((long)P4)<<35);
		var1 = ((var1 * var1 * (long)P3)>>8) + ((var1 * (long)P2)<<12);
		var1 = (((((long)1)<<47)+var1))*((long)P1)>>33;
		if (var1 == 0)
		{
			return 0; // avoid exception caused by division by zero
		}
		p = 1048576-adc_P;
		p = (((p<<31)-var2)*3125)/var1;
		var1 = (((long)P9) * (p>>13) * (p>>13)) >> 25;
		var2 = (((long)P8) * p) >> 19;
		p = ((p + var1 + var2) >> 8) + (((long)P7)<<4);
		return (uint)p;
	}

        private void HandleMeasurement()
        {
            this.Log(LogLevel.Noisy, "HandleMeasurement set {0}", (MeasurementModes)ctrlMeasurement.Value);

            var uncompensatedTemp = GetUncompensatedTemperature();
            outMSB.Value = (byte)((uncompensatedTemp >> 12) & 0xFF);
            outLSB.Value = (byte)((uncompensatedTemp >> 4) & 0xFF);
	    outXLSB.Value = (byte)((uncompensatedTemp << 4) & 0xF0);

            var uPressure = UncompensatedPressure << (byte)(8);

		ushort P1=(ushort)((coeffCalib8F.Value << 8) + coeffCalib8E.Value);
		short P2=(short)((coeffCalib91.Value << 8) + coeffCalib90.Value);
		short P3=(short)((coeffCalib93.Value << 8) + coeffCalib92.Value);
		short P4=(short)((coeffCalib95.Value << 8) + coeffCalib94.Value);
		short P5=(short)((coeffCalib97.Value << 8) + coeffCalib96.Value);
		short P6=(short)((coeffCalib99.Value << 8) + coeffCalib98.Value);
		short P7=(short)((coeffCalib9B.Value << 8) + coeffCalib9A.Value);
		short P8=(short)((coeffCalib9D.Value << 8) + coeffCalib9C.Value);
		short P9=(short)((coeffCalib9F.Value << 8) + coeffCalib9E.Value);
		long var1, var2, p;
		var1 = ((long)t_fine) - 128000;
		var2 = var1 * var1 * (long)P6;
		var2 = var2 + ((var1*(long)P5)<<17);
		var2 = var2 + (((long)P4)<<35);
		var1 = ((var1 * var1 * (long)P3)>>8) + ((var1 * (long)P2)<<12);
		var1 = (((((long)1)<<47)+var1))*((long)P1)>>33;

		double P=0;
		P = ((double)var1*(double)var1* ((-(9765625.0*(double)var2*(double)((long)P9))/(134217728.0*(double)var1*(double)var1) + (163840000000000.0*(double)((long)P9))/((double)var1*(double)var1) + (50000.0*((double)((long)P8) + 524288.0))/(double)var1) - Math.Sqrt(Math.Pow(((9765625.0*(double)var2*(double)((long)P9))/(134217728.0*(double)var1*(double)var1) - (163840000000000.0*(double)((long)P9))/((double)var1*(double)var1) - (50000.0*((double)((long)P8) + 524288.0))/(double)var1),2) - (312500000.0*(double)((long)P9)*((9765625.0*(double)var2*(double)var2*(double)((long)P9))/(576460752303423488.0*(double)var1*(double)var1) - (9765625.0*(double)var2*(double)((long)P9))/(128.0*(double)var1*(double)var1) + (85899345920000000000.0*(double)((long)P9))/((double)var1*(double)var1) - (3125.0*(double)var2*((double)((long)P8) + 524288.0))/(134217728.0*(double)var1) + (52428800000.0*((double)((long)P8) + 524288.0))/(double)var1 - (double)((uint)uPressure)+ 16.0*(double)((long)P7)))/((double)var1*(double)var1))))/(156250000.0*(double)((long)P9));
		this.Log(LogLevel.Noisy, "P set {0} {1} {2}", (int)P,BME280_compensate_P_int64((int)P), UncompensatedPressure);
 
		press_outMSB.Value = (byte)((((int)P) >> 12) & 0xFF);
                press_outLSB.Value = (byte)((((int)P) >> 4) & 0xFF);
                press_outXLSB.Value = (byte)((((int)P) << 4) & 0xF0);

            startConversion.Value = false;
            this.Log(LogLevel.Noisy, "Conversion is complete");
        }

        private IFlagRegisterField startConversion;
        private IValueRegisterField controlOversampling;
        private IValueRegisterField outMSB;
        private IValueRegisterField outLSB;
        private IValueRegisterField outXLSB;
        private IValueRegisterField press_outMSB;
        private IValueRegisterField press_outLSB;
        private IValueRegisterField press_outXLSB;
        private IValueRegisterField ctrlMeasurement;
        private Registers registerAddress;

        private IValueRegisterField coeffCalib88;
        private IValueRegisterField coeffCalib89;
        private IValueRegisterField coeffCalib8A;
        private IValueRegisterField coeffCalib8B;
        private IValueRegisterField coeffCalib8C;
        private IValueRegisterField coeffCalib8D;

        private IValueRegisterField coeffCalib8E;
        private IValueRegisterField coeffCalib8F;
        private IValueRegisterField coeffCalib90;
        private IValueRegisterField coeffCalib91;
        private IValueRegisterField coeffCalib92;
        private IValueRegisterField coeffCalib93;
        private IValueRegisterField coeffCalib94;
        private IValueRegisterField coeffCalib95;
        private IValueRegisterField coeffCalib96;
        private IValueRegisterField coeffCalib97;
        private IValueRegisterField coeffCalib98;
        private IValueRegisterField coeffCalib99;
        private IValueRegisterField coeffCalib9A;
        private IValueRegisterField coeffCalib9B;
        private IValueRegisterField coeffCalib9C;
        private IValueRegisterField coeffCalib9D;
        private IValueRegisterField coeffCalib9E;
        private IValueRegisterField coeffCalib9F;

	private IValueRegisterField status;

        private IValueRegisterField coeffCalibB2;
        private IValueRegisterField coeffCalibB3;
        private IValueRegisterField coeffCalibB4;
        private IValueRegisterField coeffCalibB5;
        private IValueRegisterField coeffCalibBC;
        private IValueRegisterField coeffCalibBD;
        private IValueRegisterField coeffCalibBE;
        private IValueRegisterField coeffCalibBF;

        private decimal temperature;
	private int t_fine;
        private const decimal MinTemperature = -40;
        private const decimal MaxTemperature = 85;
        private const decimal MinPressure = 300;
        private const decimal MaxPressure = 1100;
        private const byte resetCommand = 0xB6;
        private const short calibMB = -8711;

        private enum MeasurementModes
        {
            Temperature = 0x0E,
            Pressure    = 0x14,
        }

        private enum Registers
        {
            CoefficientCalibration88 = 0x88, // Read-Only
            CoefficientCalibration89 = 0x89,
            CoefficientCalibration8A = 0x8A,
            CoefficientCalibration8B = 0x8B,
            CoefficientCalibration8C = 0x8C,
            CoefficientCalibration8D = 0x8D,
            CoefficientCalibration8E = 0x8E,
            CoefficientCalibration8F = 0x8F,
            CoefficientCalibration90 = 0x90,
            CoefficientCalibration91 = 0x91,
            CoefficientCalibration92 = 0x92,
            CoefficientCalibration93 = 0x93,
            CoefficientCalibration94 = 0x94,
            CoefficientCalibration95 = 0x95,
            CoefficientCalibration96 = 0x96,
            CoefficientCalibration97 = 0x97,
            CoefficientCalibration98 = 0x98,
            CoefficientCalibration99 = 0x99,
            CoefficientCalibration9A = 0x9A,
            CoefficientCalibration9B = 0x9B,
            CoefficientCalibration9C = 0x9C,
            CoefficientCalibration9D = 0x9D,
            CoefficientCalibration9E = 0x9E,
            CoefficientCalibration9F = 0x9F,
            ChipID = 0xD0, // Read-Only
            SoftReset = 0xE0, // Write-Only
            CtrlMeasurement = 0xF4, // Read-Write
	    Status = 0xF3,
            OutMSB = 0xFA,  // Read-Only
            OutLSB = 0xFB,  // Read-Only
            OutXLSB = 0xFC,  // Read-Only
	    press_msb = 0xF7,
	    press_lsb = 0xF8,
            press_xlsb = 0xF9
        }
    }
}

