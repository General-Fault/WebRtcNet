#pragma once

#include <rtc_base/logging.h>
#include <msclr/marshal.h>
#include <map>
#include "MarshalEnums.h"

// Static map for rtc::LoggingSeverity to LogLevel conversion
static const std::map<const rtc::LoggingSeverity, const Microsoft::Extensions::Logging::LogLevel> rtc_logging_severity_map{
	{ rtc::LS_VERBOSE, Microsoft::Extensions::Logging::LogLevel::Debug },
	{ rtc::LS_INFO, Microsoft::Extensions::Logging::LogLevel::Information },
	{ rtc::LS_WARNING, Microsoft::Extensions::Logging::LogLevel::Warning },
	{ rtc::LS_ERROR, Microsoft::Extensions::Logging::LogLevel::Error },
	{ rtc::LS_NONE, Microsoft::Extensions::Logging::LogLevel::None }
};

// Marshal rtc::LoggingSeverity to Microsoft.Extensions.Logging.LogLevel
template <>
Microsoft::Extensions::Logging::LogLevel marshal_as<Microsoft::Extensions::Logging::LogLevel>(const rtc::LoggingSeverity& from)
{
	return marshal_mapped_native_type(rtc_logging_severity_map, from);
}
