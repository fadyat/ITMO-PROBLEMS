#include <windows.h>
#include <gl/gl.h>
#include <cmath>
#include <iostream>

using namespace std;

static int WIDTH = 1024;
static int HEIGHT = 1024;

float vertex[] = {1, 1, 0,  1, -1, 0,  -1, -1, 0,  -1, 1, 0};

float degree_x = 68, degree_z = 44.5, location_z = -8.39999;
POINTFLOAT position = {13.7699, -10.6143};

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
    else if (GetKeyState(VK_UP) < 0) {
        location_z -= 0.1;
    }
    else if (GetKeyState(VK_DOWN) < 0) {
        location_z += 0.1;
    }

    if (speed != 0) {
        position.x += float(sin(alpha) * speed);
        position.y += float(cos(alpha) * speed);
    }

    glRotatef(-degree_x, 1, 0, 0);
    glRotatef(-degree_z, 0, 0, 1);
    glTranslatef(-position.x, -position.y, location_z);
}

class little_cube {
private:
    unsigned char rgb[3];
public:
    // up down front back left right
    int color[6];
    float edge_size;
    explicit little_cube() : color(), edge_size(0), rgb() {
        memset(color, 0, sizeof(color));
    }

    // rotation of the cube relative to the XOZ plane by 90 degrees clockwise
    void rotate_x() {
        // 012345 -> 452310
        swap(color[0], color[4]); // 412305
        swap(color[1], color[5]); // 452301
        swap(color[4], color[5]); // 452310
    }

    // rotation of the cube relative to the YOZ plane by 90 degrees clockwise
    void rotate_y() {
        // 012345 -> 231045
        swap(color[0], color[2]); // 210345
        swap(color[1], color[3]); // 230145
        swap(color[2], color[3]); // 231045
    }

    // rotation of the cube relative to the XOY plane by 90 degrees clockwise
    void rotate_z() {
        // 012345 -> 015423
        swap(color[2], color[5]); // 015342
        swap(color[3], color[4]); // 015432
        swap(color[4], color[5]); // 015423
    };

    void set_color(int plane_number, int color_) {
        this->color[plane_number] = color_;
    }

    unsigned char *my_color_hex(int plane_number) {
        // divide color[plane_number] on rgb
        rgb[0] = color[plane_number] >> 16;
        rgb[1] = color[plane_number] >> 8;
        rgb[2] = color[plane_number];
        return rgb;
    }

    void draw_translated(float x = 0, float y = 0, float z = 0) {
        glPushMatrix();
        glTranslatef(x, y, z);
        glBegin(GL_QUADS);

        // up
        glColor3ubv(my_color_hex(0));
        glVertex3f(edge_size, edge_size, edge_size);
        glVertex3f(0, edge_size, edge_size);
        glVertex3f(0, 0, edge_size);
        glVertex3f(edge_size, 0, edge_size);

        // down
        glColor3ubv(my_color_hex(1));
        glVertex3f(edge_size, 0, 0);
        glVertex3f(0, 0, 0);
        glVertex3f(0, edge_size, 0);
        glVertex3f(edge_size, edge_size, 0);

        // front
        glColor3ubv(my_color_hex(2));
        glVertex3f(edge_size, 0, edge_size);
        glVertex3f(0, 0, edge_size);
        glVertex3f(0, 0, 0);
        glVertex3f(edge_size, 0, 0);

        // back
        glColor3ubv(my_color_hex(3));
        glVertex3f(edge_size, edge_size, 0);
        glVertex3f(0, edge_size, 0);
        glVertex3f(0, edge_size, edge_size);
        glVertex3f(edge_size, edge_size, edge_size);

        // left
        glColor3ubv(my_color_hex(4));
        glVertex3f(0, edge_size, edge_size);
        glVertex3f(0, edge_size, 0);
        glVertex3f(0, 0, 0);
        glVertex3f(0, 0, edge_size);

        // right
        glColor3ubv(my_color_hex(5));
        glVertex3f(edge_size, edge_size, 0);
        glVertex3f(edge_size, edge_size, edge_size);
        glVertex3f(edge_size, 0, edge_size);
        glVertex3f(edge_size, 0, 0);

        glEnd();
        glPopMatrix();
    }

    ~little_cube() = default;
};

int color_hex[9] = {0xFFFFFF, 0xFFFF00, 0xFF0F00, 0xFF931C, 0x25FF00, 0x3155EC};

class big_cube {
private:
    little_cube me[3][3][3];
    float edge_size;
public:
    explicit big_cube(float edge_size_) : edge_size(edge_size_), me() {
        // set little_cubes egde_sizes
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    me[x][y][z].edge_size = float(edge_size / 3 - 0.03);
                }
            }
        }

        // up
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                me[x][y][2].set_color(0, color_hex[0]);
            }
        }

        // down
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                me[x][y][0].set_color(1, color_hex[1]);
            }
        }

        // front
        for (int x = 0; x < 3; x++) {
            for (int z = 0; z < 3; z++) {
                me[x][0][z].set_color(2, color_hex[2]);
            }
        }

        //back
        for (int x = 0; x < 3; x++) {
            for (int z = 0; z < 3; z++) {
                me[x][2][z].set_color(3, color_hex[3]);
            }
        }

        //left
        for (int y = 0; y < 3; y++) {
            for (int z = 0; z < 3; z++) {
                me[0][y][z].set_color(4, color_hex[4]);
            }
        }

        //right
        for (int y = 0; y < 3; y++) {
            for (int z = 0; z < 3; z++) {
                me[2][y][z].set_color(5, color_hex[5]);
            }
        }
    }

    void show() {
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    me[x][y][z].draw_translated(edge_size / 3 * float(x),
                                                edge_size / 3 * float(y),
                                                edge_size / 3 * float(z));
                }
            }
        }
    }
};




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
    hwnd = CreateWindowEx(0, "GLSample", "CUB",
                          WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,
                          CW_USEDEFAULT, WIDTH, HEIGHT, nullptr,
                          nullptr, hInstance, nullptr);

    ShowWindow(hwnd, nCmdShow);

    /* enable OpenGL for the window */
    EnableOpenGL(hwnd, &hDC, &hRC);
    glEnable(GL_DEPTH_TEST);
    glFrustum(-1, 1,  -1, 1,  3, 100);
    big_cube cube(3);

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

            glClearColor(0.76953125f, 0.8125f, 0.8984375f, 0);
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            glPushMatrix();
            movement();
            cube.show();
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