// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#define NOMINMAX //some windows headers define a min and max macro that causes confusion with std::min and std::max.

#include <cstddef>
#include <cstdint>
#include <string>
#include <memory>

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;

#endif //PCH_H