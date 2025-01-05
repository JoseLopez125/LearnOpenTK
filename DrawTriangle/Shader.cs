using OpenTK.Graphics.OpenGL;

public class Shader
{
    int handle;

    public Shader(string vertexPath, string fragmentPath)
    {
        int vertexShader, fragmentShader;

        string vertexShaderSource = File.ReadAllText(vertexPath);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);

        // Create the vertex and fragment shaders and bind them to the source code
        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);

        fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        
        // Compile vertex shader
        GL.CompileShader(vertexShader);

        // Check for errors in the vertex shader compliation
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexShaderSuccess);
        if (vertexShaderSuccess == 0)
        {
            string infoLog = GL.GetShaderInfoLog(vertexShader);
            Console.WriteLine(infoLog);
        }

        // Compile fragment shader
        GL.CompileShader(fragmentShader);

        // Check for errors in the fragment shader compliation
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentShaderSuccess);
        if (fragmentShaderSuccess == 0)
        {
            string infoLog = GL.GetShaderInfoLog(fragmentShader);
            Console.WriteLine(infoLog);
        }

        // Create the shader program
        handle = GL.CreateProgram();

        // Attach the shaders to the program
        GL.AttachShader(handle, vertexShader);
        GL.AttachShader(handle, fragmentShader);

        // Link the program
        GL.LinkProgram(handle);

        // Check for errors in the program linking
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(handle);
            Console.WriteLine(infoLog);
        }

        // Delete the shaders as they are now linked to the program
        GL.DetachShader(handle, vertexShader);
        GL.DetachShader(handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(handle);
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(handle);

            disposedValue = true;
        }
    }

    ~Shader()
    {
        if (disposedValue == false)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}