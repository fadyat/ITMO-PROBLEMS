#include <iostream>
#include <glad/glad.h>
#include <GLFW/glfw3.h>

using namespace std;

static const int WIDTH = 800;
static const int HEIGHT = 600;

int main()
{
    // Init GLFW
    if (!glfwInit()) {
        cout << "glfwInit()\n";
        return -1;
    }
    /* Options */
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 6);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    glfwWindowHint(GLFW_RESIZABLE, GL_FALSE);

    /* Window */
    GLFWwindow* window = glfwCreateWindow(WIDTH, HEIGHT, "LearnOpenGL", nullptr, nullptr);
    if (!window) {
        cout << "window\n";
        glfwTerminate();
        return -1;
    }
    glfwMakeContextCurrent(window);

    /* GL */
    if (!gladLoadGL()) {
        cout << "gladLoadGL()\n";
        return -1;
    }


    while (!glfwWindowShouldClose(window))
    {
        glfwPollEvents();

        glClearColor(0.3f, 0.3f, 0.3f, 1.0f);

        glClear(GL_COLOR_BUFFER_BIT);

        glfwSwapBuffers(window);
    }


    glfwTerminate();
    return 0;
}