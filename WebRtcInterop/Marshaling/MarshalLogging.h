#pragma once

#include <rtc_base/logging.h>
#include <msclr/marshal.h>
#include <map>
#include "MarshalEnums.h"

// Static map for rtc::LoggingSeverity to LogLevel conversion
static const std::map<const rtc::LoggingSeverity, const System::Diagnostics::LogLevel> rtc_logging_severity_map{
	{ rtc::LS_VERBOSE, System::Diagnostics::LogLevel::Debug },
	{ rtc::LS_INFO, System::Diagnostics::LogLevel::Information },
	{ rtc::LS_WARNING, System::Diagnostics::LogLevel::Warning },
	{ rtc::LS_ERROR, System::Diagnostics::LogLevel::Error },
	{ rtc::LS_NONE, System::Diagnostics::LogLevel::None }
};

// Marshal rtc::LoggingSeverity to System::Diagnostics::LogLevel
template <>
System::Diagnostics::LogLevel marshal_as<System::Diagnostics::LogLevel>(const rtc::LoggingSeverity& from)
{
	return marshal_mapped_native_type(rtc_logging_severity_map, from);
}
