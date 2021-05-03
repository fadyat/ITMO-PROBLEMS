cmake_minimum_required(VERSION 3.19)
project(roads)

set(CMAKE_CXX_STANDARD 17)

add_executable(roads main.cpp pugiconfig.hpp pugixml.cpp pugixml.hpp)