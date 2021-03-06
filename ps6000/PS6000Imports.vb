﻿'===================================================================================================
'
'	Filename:			PS6000Imports.vb
'
'	Description: 
'	This file defines enumerations, functions and structures from the ps6000Api.h C header file.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Module PS6000Imports

    ' Constant values
    ' ===============

    Public Const PS6000_MAX_VALUE = 32512          ' MAX ADC value used when converting between ADC values and mV

    Public Const AWG_BUFFER_SIZE_16KS = 16384      ' 6000A/B AWG Buffer Size
    Public Const AWG_BUFFER_SIZE_64KS = 65536      ' 6000C/D AWG Buffer Size


    ' Enumerations
    ' ============

    ' ModelType not defined in ps6000Api.h

    Enum ModelType As UInteger
        MODEL_NONE = 0
        MODEL_PS6402A = &HA402
        MODEL_PS6402B = &HB402
        MODEL_PS6402C = &HC402
        MODEL_PS6402D = &HD402
        MODEL_PS6403A = &HA403
        MODEL_PS6403B = &HB403
        MODEL_PS6403C = &HC403
        MODEL_PS6403D = &HD403
        MODEL_PS6404A = &HA404
        MODEL_PS6404B = &HB404
        MODEL_PS6404C = &HC404
        MODEL_PS6404D = &HD404
        MODEL_PS6407 = 6407
    End Enum

    Enum BandwidthLimiter
        PS6000_BW_FULL
        PS6000_BW_20MHZ
        PS6000_BW_25MHZ
    End Enum

    Enum Channel
        PS6000_CHANNEL_A
        PS6000_CHANNEL_B
        PS6000_CHANNEL_C
        PS6000_CHANNEL_D
        PS6000_EXTERNAL
    End Enum

    Enum Coupling
        PS6000_AC
        PS6000_DC_1M
        PS6000_DC_50R
    End Enum

    Enum InfoType
        PICO_DRIVER_VERSION
        PICO_USB_VERSION
        PICO_HARDWARE_VERSION
        PICO_VARIANT_INFO
        PICO_BATCH_AND_SERIAL
        PICO_CAL_DATE
        PICO_KERNEL_VERSION
    End Enum

    Enum SigGenType
        NONE
        FUNCT_GEN
        AWG
    End Enum

    Enum ThresholdMode
        LEVEL
        WINDOW
    End Enum

    Enum TriggerDirection
        'Above=0, Below=1, Rising / None=2, Falling=3, Rising_Or_Falling=4, Above_Lower=5, Below_Lower=6, Rising_Lower=7, Falling_Lower=8,
        ABOVE
        BELOW
        RISING
        NONE = RISING
        FALLING
        RISING_OR_FALLING
        ABOVE_LOWER
        BELOW_LOWER
        RISING_LOWER
        FALLING_LOWER
        ' Window triggers
        INSIDE = ABOVE
        OUTSIDE = BELOW
        WINDOW_ENTER = RISING
        WINDOW_EXIT = FALLING
        WINDOW_ENTER_OR_EXIT = RISING_OR_FALLING
        POSITIVE_RUNT = 9
        NEGATIVE_RUNT
    End Enum

    Enum TriggerState
        CONDITION_DONT_CARE
        CONDITION_TRUE
        CONDITION_FALSE
        CONDITION_MAX
    End Enum

    Enum RatioMode
        PS6000_RATIO_MODE_NONE
        PS6000_RATIO_MODE_AGGREGATE = 1
        PS6000_RATIO_MODE_AVERAGE = 2
        PS6000_RATIO_MODE_DECIMATE = 4
        'PS6000_RATIO_MODE_DISTRIBUTION = 8 - Not Supported
    End Enum

    Enum VoltageRange
        PS6000_10MV
        PS6000_20MV
        PS6000_50MV
        PS6000_100MV
        PS6000_200MV
        PS6000_500MV
        PS6000_1V
        PS6000_2V
        PS6000_5V
        PS6000_10V
        PS6000_20V
        PS6000_50V
        PS6000_100V
    End Enum

    Enum WaveType
        PS6000_SINE
        PS6000_SQUARE
        PS6000_TRIANGLE
        PS6000_RAMP_UP
        PS6000_RAMP_DOWN
        PS6000_SINC
        PS6000_GAUSSIAN
        PS6000_HALF_SINE
        PS6000_DC_VOLTAGE
        PS6000_MAX_WAVE_TYPES
    End Enum


    ' Structures
    ' ==========

    Structure Channel_SETTINGS
        Dim dcCoupled As Coupling
        Dim range As VoltageRange
        Dim enabled As Boolean
        Dim analogueOffset As Single
    End Structure

    Structure UNIT_MODEL
        Dim handle As Short
        Dim model As ModelType
        Dim channelCount As Short
        Dim firstRange As VoltageRange
        Dim lastRange As VoltageRange
        Dim channelSettings() As Channel_SETTINGS
        Dim sigGenType As Boolean
        Dim sigGenBufferSize As UInteger
    End Structure

    Structure TRIGGER_CHANNEL_PROPERTIES
        Dim thresholdUpper As Short
        Dim thresholdUpperHysteresis As UShort
        Dim thresholdLower As Short
        Dim thresholdLowerHysteresis As UShort
        Dim Channel As Integer
        Dim thresholdMode As Integer
    End Structure

    Structure TRIG_SETTINGS
        Dim ChanTrig() As Boolean
        Dim NumberTrigs As Short
        'Dim TrigType As TrigLogic
        Dim TrigVoltage As Short
        Dim TrigHist As Short
    End Structure


    ' Function Declarations
    ' =====================

    ' Device Connection and Setup Functions
    ' -------------------------------------

    Declare Function ps6000OpenUnit Lib "ps6000.dll" (ByRef handle As Short, ByVal serial As String) As UInteger
    Declare Sub ps6000CloseUnit Lib "ps6000.dll" (ByVal handle As Short)

    Declare Function ps6000GetUnitInfo Lib "ps6000.dll" (ByVal handle As Short, ByVal str As String, ByVal strlength As Short, ByRef requiredSize As Short, ByVal info As InfoType) As UInteger

    Declare Function ps6000SetChannel Lib "ps6000.dll" (ByVal handle As Short, ByVal channel As Channel, ByVal enabled As Short, ByVal type As Coupling, ByVal range As VoltageRange, ByVal analogueOffset As Single, ByVal bandwidth As BandwidthLimiter) As UInteger


    ' Block functions
    ' ---------------

    Declare Function ps6000GetTimebase2 Lib "ps6000.dll" (ByVal handle As Short, ByVal timebase As UInteger, ByVal noSamples As UInteger, ByRef timeIntervalNs As Single, ByVal oversample As Short, ByRef maxSamples As UInteger, ByVal segmentIndex As UInteger) As UInteger
    Declare Function ps6000RunBlock Lib "ps6000.dll" (ByVal handle As Short, noOfPreTriggerSamples As UInteger, noOfPostTriggerSamples As UInteger, timebase As UInteger, oversample As Short, ByRef timeIndisposedMs As Integer, ByVal segmentIndex As UInteger, ByVal lpps6000BlockReady As ps6000BlockReady, pParam As IntPtr) As UInteger
    Declare Function ps6000IsReady Lib "ps6000.dll" (ByVal handle As Short, ByRef ready As Short) As UInteger
    Declare Function ps6000GetValues Lib "ps6000.dll" (ByVal handle As Short, ByVal startIndex As UInteger, ByRef numSamples As UInteger, ByVal downSampleRatio As UInteger, ByVal downSampleRatioMode As RatioMode, ByVal segmentIndex As UInteger, ByRef overflow As Short) As UInteger


    ' Trigger Functions
    ' -----------------
    Declare Function ps6000SetSimpleTrigger Lib "ps6000.dll" (ByVal handle As Short, ByVal enable As Short, ByVal source As Channel, ByVal threshold As Short, ByVal direction As TriggerDirection, ByVal delay As UInteger, ByVal autoTriggerMs As Short) As UInteger

    ' Functions relevant to all Data Capture Modes
    ' --------------------------------------------

    Declare Function ps6000SetDataBuffer Lib "ps6000.dll" (ByVal handle As Short, ByVal channel As Channel, ByRef buffer As Short, ByVal length As UInteger, ByVal downSampleRatioMode As RatioMode) As UInteger
    Declare Function ps6000Stop Lib "ps6000.dll" (ByVal handle As Short) As UInteger


    ' Delegate declarations
    ' =====================

    ' Block mode
    ' ----------
    Public Delegate Sub ps6000BlockReady(handle As Short, status As UInteger, pVoid As IntPtr)


    ' Other Functions
    ' ===============

    Declare Sub Sleep Lib "kernel32.dll" (ByVal time As Short)

End Module
