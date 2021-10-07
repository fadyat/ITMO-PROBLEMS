cmake_minimum_required(VERSION 3.17)

set(PROJECT_NAME lab6)
project(${PROJECT_NAME})

add_executable(lab6 src/main.cpp )

set(GLFW_BUILD_DOCS OFF CACHE BOOL "" FORCE)
set(GLFW_BUILD_TESTS OFF CACHE BOOL "" FORCE)
set(GLFW_BUILD_EXAMPLES OFF CACHE BOOL "" FORCE)
set(GLFW_INSTALL OFF CACHE BOOL "" FORCE)

add_subdirectory(external/glfw)
target_link_libraries(${PROJECT_NAME} glfw)

add_subdirectory(external/glad)
target_link_libraries(${PROJECT_NAME} glad)

target_link_libraries(${PROJECT_NAME} opengl32 freeglut glu32)