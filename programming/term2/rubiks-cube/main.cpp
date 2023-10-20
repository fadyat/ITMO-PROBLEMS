#include <windows.h>
#include <gl/gl.h>
#include <cmath>
#include <iostream>

using namespace std;

static int WIDTH = 1024;
static int HEIGHT = 1024;

float angle_x = 68, angle_z = 44.5, location_z = -8.39999;
POINTFLOAT position = {13.7699, -10.6143};

void movement() {
    /* move camera */
    if(GetKeyState('I') < 0) {
        angle_x += 0.5;
    }
    else if(GetKeyState('K') < 0) {
        angle_x -= 0.5;
    }
    else if(GetKeyState('J') < 0) {
        angle_z += 0.5;
    }
    else if(GetKeyState('L') < 0) {
        angle_z -= 0.5;
    }

    /* move me */
    double alpha = -angle_z / 180 * M_PI;
    float speed = 0;
    if (GetKeyState('S') < 0) {
        speed = 0.1;
    }
    else if (GetKeyState('X') < 0) {
        speed = -0.1;
    }
    else if (GetKeyState('Z') < 0) {
        speed = -0.1;
        alpha = M_PI * 0.5;
    }
    else if (GetKeyState('C') < 0) {
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

    glRotatef(-angle_x, 1, 0, 0);
    glRotatef(-angle_z, 0, 0, 1);
    glTranslatef(-position.x, -position.y, location_z);
}

/*
 *  The planes of the cube go in the order:
 *      top     down    front   back    left    right
 */

int color_hex[9] = {0xFFFFFF, 0xFFFF00, 0xFF0F00, 0xFF931C, 0x25FF00, 0x3155EC};

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

    /* rotation of the cube relative to the *** plane by 90 angles clockwise */
    void rotate_z() {
        // 012345 -> 015423
        swap(color[2], color[5]); // 015342
        swap(color[3], color[4]); // 015432
        swap(color[4], color[5]); // 015423
    };

    void rotate_y() {
        // 012345 -> 231045
        swap(color[0], color[2]); // 210345
        swap(color[1], color[3]); // 230145
        swap(color[2], color[3]); // 231045
    }

    void rotate_x() {
        // 012345 -> 452310
        swap(color[0], color[4]); // 412305
        swap(color[1], color[5]); // 452301
        swap(color[4], color[5]); // 452310
    }

    void set_color(int plane_number, int color_) {
        this->color[plane_number] = color_;
    }

    unsigned char *my_color_hex(int plane_number) {
        rgb[0] = color[plane_number] >> 16;
        rgb[1] = color[plane_number] >> 8;
        rgb[2] = color[plane_number];
        return rgb;
    }

    void draw_translated(float x = 0, float y = 0, float z = 0) {
        glPushMatrix();
        glTranslatef(x, y, z);
        glBegin(GL_QUADS);

        glColor3ubv(my_color_hex(0));
        glVertex3f(edge_size, edge_size, edge_size);
        glVertex3f(0, edge_size, edge_size);
        glVertex3f(0, 0, edge_size);
        glVertex3f(edge_size, 0, edge_size);

        glColor3ubv(my_color_hex(1));
        glVertex3f(edge_size, 0, 0);
        glVertex3f(0, 0, 0);
        glVertex3f(0, edge_size, 0);
        glVertex3f(edge_size, edge_size, 0);

        glColor3ubv(my_color_hex(2));
        glVertex3f(edge_size, 0, edge_size);
        glVertex3f(0, 0, edge_size);
        glVertex3f(0, 0, 0);
        glVertex3f(edge_size, 0, 0);

        glColor3ubv(my_color_hex(3));
        glVertex3f(edge_size, edge_size, 0);
        glVertex3f(0, edge_size, 0);
        glVertex3f(0, edge_size, edge_size);
        glVertex3f(edge_size, edge_size, edge_size);

        glColor3ubv(my_color_hex(4));
        glVertex3f(0, edge_size, edge_size);
        glVertex3f(0, edge_size, 0);
        glVertex3f(0, 0, 0);
        glVertex3f(0, 0, edge_size);

        glColor3ubv(my_color_hex(5));
        glVertex3f(edge_size, edge_size, 0);
        glVertex3f(edge_size, edge_size, edge_size);
        glVertex3f(edge_size, 0, edge_size);
        glVertex3f(edge_size, 0, 0);

        glEnd();
        glPopMatrix();
    }
};

class big_cube {
private:
    little_cube me[3][3][3];
    little_cube tmp_face[3][3];
    bool fixed[3][3][3];
    float angle_of_face[6];
    float edge_size;
    float angle;
    int cube_face;

public:
    explicit big_cube(float edge_size_) : edge_size(edge_size_), me(), fixed(), cube_face(-1), angle_of_face(), angle(3) {
        memset(angle_of_face, 0, sizeof(angle_of_face));
        // set little_cubes egde_sizes
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    me[x][y][z].edge_size = float(edge_size / 3 - 0.03);
                }
            }
        }

        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                me[x][y][2].set_color(0, color_hex[0]);
                me[x][y][0].set_color(1, color_hex[1]);
            }
        }

        for (int x = 0; x < 3; x++) {
            for (int z = 0; z < 3; z++) {
                me[x][0][z].set_color(2, color_hex[2]);
                me[x][2][z].set_color(3, color_hex[3]);
            }
        }

        for (int y = 0; y < 3; y++) {
            for (int z = 0; z < 3; z++) {
                me[0][y][z].set_color(4, color_hex[4]);
                me[2][y][z].set_color(5, color_hex[5]);
            }
        }
    }

    void show() {
        memset(fixed, true, sizeof(fixed));

        glPushMatrix();
        int u = (cube_face & 1) * 2;
        // XOY
        if (cube_face == 0 || cube_face == 1) {
            for (int x = 0; x < 3; x++) {
                for (int y = 0; y < 3; y++) {
                    fixed[x][y][u] = false;
                }
            }
            glTranslatef(edge_size / 2, edge_size / 2, 0);
            glRotatef(float(angle_of_face[cube_face]), 0, 0, 1);
            glTranslatef(-edge_size / 2, -edge_size / 2, 0);
            for (int x = 0; x < 3; x++) {
                for (int y = 0; y < 3; y++) {
                    me[x][y][u].draw_translated(edge_size * float(x) / 3,
                                                edge_size * float(y) / 3,
                                                edge_size * float(u) / 3);
                }
            }
        }
        // XOZ
        else if (cube_face == 2 || cube_face == 3) {
            for (int x = 0; x < 3; x++) {
                for (int z = 0; z < 3; z++) {
                    fixed[x][u][z] = false;
                }
            }
            glTranslatef(edge_size / 2, 0, edge_size / 2);
            glRotatef(float(angle_of_face[cube_face]), 0, 1, 0);
            glTranslatef(-edge_size / 2, 0, -edge_size / 2);
            for (int x = 0; x < 3; x++) {
                for (int z = 0; z < 3; z++) {
                    me[x][u][z].draw_translated(edge_size * float(x) / 3,
                                                edge_size * float(u) / 3,
                                                edge_size * float(z) / 3);
                }
            }
        }
        // YOZ
        else if (cube_face == 4 || cube_face == 5) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    fixed[u][y][z] = false;
                }
            }
            glTranslatef(0, edge_size / 2, edge_size / 2);
            glRotatef(float(angle_of_face[cube_face]), 1, 0, 0);
            glTranslatef(0, -edge_size / 2, -edge_size / 2);
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    me[u][y][z].draw_translated(edge_size * float(u) / 3,
                                                edge_size * float(y) / 3,
                                                edge_size * float(z) / 3);
                }
            }
        }
        glPopMatrix();

        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    if(fixed[x][y][z]) {
                        me[x][y][z].draw_translated(edge_size / 3 * float(x),
                                                    edge_size / 3 * float(y),
                                                    edge_size / 3 * float(z));
                    }
                }
            }
        }
    }

    void cube_rotations() {
        int key = 0;
        if(GetKeyState('1') < 0) {
            key = 1;
        }
        else if(GetKeyState('2') < 0) {
            key = 2;
        }
        else if(GetKeyState('3') < 0) {
            key = 3;
        }
        else if(GetKeyState('4') < 0) {
            key = 4;
        }
        else if(GetKeyState('5') < 0) {
            key = 5;
        }
        else if(GetKeyState('6') < 0) {
            key = 6;
        }
        else if (GetKeyState('Q') < 0) {
            key = 7;
        }
        else if (GetKeyState('W') < 0) {
            key = 8;
        }
        else if (GetKeyState('E') < 0) {
            key = 9;
        }
        else if (GetKeyState('R') < 0) {
            key = 10;
        }
        else if (GetKeyState('T') < 0) {
            key = 11;
        }
        else if (GetKeyState('Y') < 0) {
            key = 12;
        }
        bool sign = true;
        if (key > 6) {
            key -= 6;
            sign = false;
        }

        rotate_face(--key, sign);
    }

    void rotate_face(int face_number, bool sign) {
        if (cube_face == -1 || cube_face == face_number) {
            float angle_ = angle;
            if(!sign) {
                angle_ = -angle;
            }
            angle_of_face[face_number] += angle_;
            if (int(angle_of_face[face_number]) % 90 != 0) {
                cube_face = face_number;
            }
            else {
                if (angle_of_face[face_number] > 0) {
                    int swaps = ((cube_face == 2 || cube_face == 3) ? (1) : (3));
                    is_full_rotate(face_number, swaps);
                }
                else {
                    int swaps = ((cube_face == 2 || cube_face == 3) ? (3) : (1));
                    is_full_rotate(face_number, swaps);
                }
            }
        }
    }

    void is_full_rotate(int face_number, int color_changes) {
        while(color_changes--) {
            int u = (face_number & 1) * 2;
            if (face_number == 0 || face_number == 1) {
                for (int x = 0; x < 3; x++) {
                    for (int y = 0; y < 3; y++) {
                        tmp_face[y][2 - x] = me[x][y][u];
                    }
                }
                for (int x = 0; x < 3; x++) {
                    for (int y = 0; y < 3; y++) {
                        tmp_face[x][y].rotate_z();
                        me[x][y][u] = tmp_face[x][y];
                    }
                }
            }
            else if (face_number == 2 || face_number == 3) {
                for (int x = 0; x < 3; x++) {
                    for (int z = 0; z < 3; z++) {
                        tmp_face[z][2 - x] = me[x][u][z];
                    }
                }
                for (int x = 0; x < 3; x++) {
                    for (int z = 0; z < 3; z++) {
                        tmp_face[x][z].rotate_x();
                        me[x][u][z] = tmp_face[x][z];
                    }
                }
            }
            else if (face_number == 4 || face_number == 5) {
                for (int y = 0; y < 3; y++) {
                    for (int z = 0; z < 3; z++) {
                        tmp_face[z][2 - y] = me[u][y][z];
                    }
                }
                for (int y = 0; y < 3; y++) {
                    for (int z = 0; z < 3; z++) {
                        tmp_face[y][z].rotate_y();
                        me[u][y][z] = tmp_face[y][z];
                    }
                }
            }
        }
        angle_of_face[face_number] = 0;
        cube_face = -1;
    }

    void while_not_90() {
        if(cube_face != -1) {
            rotate_face(cube_face, angle_of_face[cube_face] > 0);
        }
        show();
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
    *hDC = GetDC(hwnd);
    ZeroMemory(&pfd, sizeof(pfd));
    pfd.nSize = sizeof(pfd);
    pfd.nVersion = 1;
    pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
    pfd.iPixelType = PFD_TYPE_RGBA;
    pfd.cColorBits = 24;
    pfd.cDepthBits = 16;
    pfd.iLayerType = PFD_MAIN_PLANE;
    iFormat = ChoosePixelFormat(*hDC, &pfd);
    SetPixelFormat(*hDC, iFormat, &pfd);
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

    hwnd = CreateWindowEx(0, "GLSample", "CUB",
                          WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,
                          CW_USEDEFAULT, WIDTH, HEIGHT, nullptr,
                          nullptr, hInstance, nullptr);

    ShowWindow(hwnd, nCmdShow);


    EnableOpenGL(hwnd, &hDC, &hRC);
    glEnable(GL_DEPTH_TEST);
    glFrustum(-1, 1,  -1, 1,  3, 100);
    big_cube cube(3);

    while (!bQuit) {
        if (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE)) {
            if (msg.message == WM_QUIT) {
                bQuit = TRUE;
            }
            else {
                TranslateMessage(&msg);
                DispatchMessage(&msg);
            }
        }
        else {
            glClearColor(0.76953125f, 0.8125f, 0.8984375f, 0);
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            glPushMatrix();
            movement();
            cube.while_not_90();
            cube.cube_rotations();
            glPopMatrix();

            SwapBuffers(hDC);
        }
    }
    DisableOpenGL(hwnd, hDC, hRC);
    DestroyWindow(hwnd);
    return (int) msg.wParam;
}