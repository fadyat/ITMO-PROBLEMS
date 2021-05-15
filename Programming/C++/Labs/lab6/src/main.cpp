#include <windows.h>
#include <gl/gl.h>
#include <cmath>
#include <iostream>

using namespace std;

static int WIDTH = 1024;
static int HEIGHT = 1024;

float vertex[] = {1, 1, 0,  1, -1, 0,  -1, -1, 0,  -1, 1, 0};

void show_ground() {
    glEnableClientState(GL_VERTEX_ARRAY);
        glVertexPointer(3, GL_FLOAT, 0, &vertex);
        for (int i = -5; i < 5; i++) {
            for (int j = -5; j < 5; j++) {
                glPushMatrix();
                    ((i + j) % 2) ? (glColor3f(1, 1, 1)) : (glColor3f(0.3, 0.3, 0.3));
                    glTranslatef(float(2 * i), float(2 * j), 0);
                    glDrawArrays(GL_TRIANGLE_FAN, 0, 4);
                glPopMatrix();
            }
        }
    glDisable(GL_VERTEX_ARRAY);
}

double degree_x = 67, degree_z = 6;
POINTFLOAT position = {2.5, -31.4001};

void movement() {
    /* move camera */
    if(GetKeyState('I') < 0) {
        degree_x += 0.5;
    }
    else if(GetKeyState('K') < 0) {
        degree_x -= 0.5;
    }
    else if(GetKeyState('J') < 0) {
        degree_z += 0.5;
    }
    else if(GetKeyState('L') < 0) {
        degree_z -= 0.5;
    }

    /* move me */
    double alpha = -degree_z / 180 * M_PI;
    float speed = 0;
    if (GetKeyState('W') < 0) {
        speed = 0.1;
    }
    else if (GetKeyState('S') < 0) {
        speed = -0.1;
    }
    else if (GetKeyState('A') < 0) {
        speed = -0.1;
        alpha = M_PI * 0.5;
    }
    else if (GetKeyState('D') < 0) {
        speed = 0.1;
        alpha = M_PI * 0.5;
    }
    if (speed != 0) {
        position.x += float(sin(alpha) * speed);
        position.y += float(cos(alpha) * speed);
    }
    glRotatef(float(-degree_x), 1, 0, 0);
    glRotatef(float(-degree_z), 0, 0, 1);
    glTranslatef(-position.x, -position.y, -10);
}



LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
    switch (uMsg) {
        case WM_CLOSE:
            PostQuitMessage(0);
            break;

        case WM_DESTROY:
            return 0;

        case WM_KEYDOWN: {
            switch (wParam) {
                case VK_ESCAPE:
                    PostQuitMessage(0);
                    break;
            }
        }
            break;

        default:
            return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }

    return 0;
}

void EnableOpenGL(HWND hwnd, HDC *hDC, HGLRC *hRC) {
    PIXELFORMATDESCRIPTOR pfd;

    int iFormat;

    /* get the device context (DC) */
    *hDC = GetDC(hwnd);

    /* set the pixel format for the DC */
    ZeroMemory(&pfd, sizeof(pfd));

    pfd.nSize = sizeof(pfd);
    pfd.nVersion = 1;
    pfd.dwFlags = PFD_DRAW_TO_WINDOW |
                  PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
    pfd.iPixelType = PFD_TYPE_RGBA;
    pfd.cColorBits = 24;
    pfd.cDepthBits = 16;
    pfd.iLayerType = PFD_MAIN_PLANE;

    iFormat = ChoosePixelFormat(*hDC, &pfd);

    SetPixelFormat(*hDC, iFormat, &pfd);

    /* create and enable the render context (RC) */
    *hRC = wglCreateContext(*hDC);

    wglMakeCurrent(*hDC, *hRC);
}

void DisableOpenGL(HWND hwnd, HDC hDC, HGLRC hRC) {
    wglMakeCurrent(nullptr, nullptr);
    wglDeleteContext(hRC);
    ReleaseDC(hwnd, hDC);
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow) {
    WNDCLASSEX wcex;
    HWND hwnd;
    HDC hDC;
    HGLRC hRC;
    MSG msg;
    BOOL bQuit = FALSE;

    /* register window class */
    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.style = CS_OWNDC;
    wcex.lpfnWndProc = WindowProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(nullptr, IDI_APPLICATION);
    wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH) GetStockObject(BLACK_BRUSH);
    wcex.lpszMenuName = nullptr;
    wcex.lpszClassName = "GLSample";
    wcex.hIconSm = LoadIcon(nullptr, IDI_APPLICATION);


    if (!RegisterClassEx(&wcex)) {
        return 0;
    }

    /* create main window */
    hwnd = CreateWindowEx(0, "GLSample", "OpenGL Sample",
                          WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,
                          CW_USEDEFAULT, WIDTH, HEIGHT, nullptr,
                          nullptr, hInstance, nullptr);

    ShowWindow(hwnd, nCmdShow);

    /* enable OpenGL for the window */
    EnableOpenGL(hwnd, &hDC, &hRC);


    glFrustum(-1, 1,  -1, 1,  2, 100);

    /* program main loop */
    while (!bQuit) {
        /* check for messages */
        if (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE)) {
            /* handle or dispatch messages */
            if (msg.message == WM_QUIT) {
                bQuit = TRUE;
            } else {
                TranslateMessage(&msg);
                DispatchMessage(&msg);
            }
        } else {
            /* OpenGL animation code goes here */

            glClearColor(0, 0, 0, 0);
            glClear(GL_COLOR_BUFFER_BIT);

            glPushMatrix();
                movement();
                show_ground();
            glPopMatrix();

            SwapBuffers(hDC);
        }
    }

    /* shutdown OpenGL */
    DisableOpenGL(hwnd, hDC, hRC);

    /* destroy the window explicitly */
    DestroyWindow(hwnd);

    return (int) msg.wParam;
}