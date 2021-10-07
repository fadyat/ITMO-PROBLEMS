GLFW: https://github.com/glfw/glfw
GLAD: https://github.com/Dav1dde/glad
  git: https://git-scm.com/
  CMake: https://cmake.org/
How can I add them to my project?
You can look there: https://www.youtube.com/playlist?list=PL6x9Hnsyqn2XU7vc8-oFLojbibK91fVd- 

...

glut: http://freeglut.sourceforge.net/
glut ---> merge with MinGW ---> change CMakeLists.txt with that:  
  target_link_libraries(${PROJECT_NAME} -lOpenGL32 -lfreeGLUT)
  
...

