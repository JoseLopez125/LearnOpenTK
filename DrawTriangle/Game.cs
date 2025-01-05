using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Window : GameWindow
{
    int vertexBufferObject;
    int vertexArrayObject;

    // Vertices for a triangle
    float[] vertices = {
        -0.5f, -0.5f, 0.0f, //Bottom-left vertex
        0.5f, -0.5f, 0.0f, //Bottom-right vertex
        0.0f,  0.5f, 0.0f  //Top vertex
    };

    Shader shader;

    public Window(int clientWidth, int clientHeight, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (clientWidth, clientHeight), Title = title, Flags = ContextFlags.ForwardCompatible }) // Flags are for macOS compatibility
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        // Generate a Vertex Buffer Object (VBO) and bind it
        vertexBufferObject = GL.GenBuffer();
        // Bind the VBO to the GL_ARRAY_BUFFER target
        // This means that any call to GL_ARRAY_BUFFER will be used to configure the VBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        // Copy the vertices data into the VBO (the size of the data is in bytes)
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        shader = new Shader("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");

        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);

        // Tell OpenGL how to interpret the vertex data (per vertex attribute)
        // The first parameter is the location of the input variable in the vertex shader (location = 0)
        // The second parameter is the number of components per generic vertex attribute. In this case, we have a vec3 in the vertex shader.
        // The third parameter is the data type of each component in the array.
        // The fourth parameter specifies whether the data should be normalized.
        // The fifth parameter is the stride (the space between consecutive vertex attributes).
        // The last parameter is the offset of where the position data begins in the buffer.
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        GL.BindVertexArray(vertexArrayObject);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    // Now, for cleanup.
    // You should generally not do cleanup of opengl resources when exiting an application,
    // as that is handled by the driver and operating system when the application exits.
    // 
    // There are reasons to delete opengl resources, but exiting the application is not one of them.
    // This is provided here as a reference on how resource cleanup is done in opengl, but
    // should not be done when exiting the application.
    //
    // Places where cleanup is appropriate would be: to delete textures that are no
    // longer used for whatever reason (e.g. a new scene is loaded that doesn't use a texture).
    // This would free up video ram (VRAM) that can be used for new textures.
    //
    // The following code is just a template.
    //
    // protected override void OnUnload()
    // {
    //     // Unbind all the resources by binding the targets to 0/null.
    //     GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    //     GL.BindVertexArray(0);
    //     GL.UseProgram(0);

    //     // Delete all the resources.
    //     GL.DeleteBuffer(vertexBufferObject);
    //     GL.DeleteVertexArray(vertexArrayObject);

    //     shader.Dispose();

    //     base.OnUnload();
    // }
}
