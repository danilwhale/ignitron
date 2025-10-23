using System.Runtime.CompilerServices;
using System.Text;
using Allumeria;
using Allumeria.Rendering;
using Ignitron.Aluminium.Assets.Descriptors;
using OpenTK.Graphics.OpenGL;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class ShaderAssetProvider : IAssetProvider<Shader, ShaderAssetDescriptor>
{
    public static ShaderAssetProvider Default { get; } = new();

    public Shader Create(AssetManager assets, string assetName, ShaderAssetDescriptor descriptor)
    {
        string name = Path.GetFileNameWithoutExtension(assetName);
        
        // jank++
        Shader shader = (Shader)RuntimeHelpers.GetUninitializedObject(typeof(Shader));

        // here goes decompiled code with locals named by me
        Logger.Verbose("Reading vertex shader file ");
        string vertexText;
        using (Stream vertexStream = assets.Open(descriptor.VertexAssetName ?? name + ".vert"))
        using (StreamReader vertexReader = new(vertexStream, Encoding.UTF8))
        {
            vertexText = vertexReader.ReadToEnd();
        }
        Logger.Verbose("Vertex Shader: " + vertexText);
        
        Logger.Verbose("Reading fragment shader file ");
        string fragmentText;
        using (Stream fragmentStream = assets.Open(descriptor.FragmentAssetName ?? name + ".frag"))
        using (StreamReader fragmentReader = new(fragmentStream, Encoding.UTF8))
        {
            fragmentText = fragmentReader.ReadToEnd();
        }
        Logger.Verbose("Vertex Shader: " + fragmentText);
        
        Logger.Verbose("Creating vertex shader ");
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        if (vertexShader == 0)
        {
            Logger.Error("Invalid vertex shader");
        }

        Logger.Verbose("Vertex shader ID: " + vertexShader);
        Logger.Verbose("Shader Source");
        GL.ShaderSource(vertexShader, vertexText);
        
        Logger.Verbose("Creating fragment shader");
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        if (fragmentShader == 0)
        {
            Logger.Error("Invalid fragment shader");
        }

        Logger.Verbose("Fragment shader ID: " + fragmentShader);
        Logger.Verbose("Shader Source");
        GL.ShaderSource(fragmentShader, fragmentText);
        
        Logger.Verbose("Compiling vertex shader on: " + Thread.CurrentThread.Name);
        GL.CompileShader(vertexShader);
        
        Logger.Verbose("Get Vertex Shader");
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexStatus);
        Logger.Verbose("Vertex shader status: " + vertexStatus);
        string vertexLog = GL.GetShaderInfoLog(vertexShader);
        if (vertexStatus == 0) Logger.Error(vertexLog);
        else Logger.Verbose(vertexLog);

        Logger.Verbose("Compiling vertex shader on: " + Thread.CurrentThread.Name);
        GL.CompileShader(fragmentShader);
        
        Logger.Verbose("Get Fragment Shader");
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentStatus);
        string fragmentLog = GL.GetShaderInfoLog(fragmentShader);
        if (fragmentStatus == 0) Logger.Error(fragmentLog);
        else Logger.Verbose(fragmentLog);
        Logger.Verbose("Fragment shader status: " + vertexStatus);
        
        Logger.Verbose("Creating shader program");
        shader.Handle = GL.CreateProgram();
        if (shader.Handle == 0) Logger.Error("Invalid shader handle");
        Logger.Verbose("Shader handle: " + shader.Handle);
        
        Logger.Verbose($"Attach Vertex Shader {shader.Handle}, {vertexShader}");
        GL.AttachShader(shader.Handle, vertexShader);
        
        Logger.Verbose($"Attach Fragment Shader {shader.Handle}, {fragmentShader}");
        GL.AttachShader(shader.Handle, fragmentShader);
        
        Logger.Verbose($"Link Program {shader.Handle}");
        GL.LinkProgram(shader.Handle);
        
        Logger.Verbose($"Get Program {shader.Handle}");
        GL.GetProgram(shader.Handle, GetProgramParameterName.LinkStatus, out int linkStatus);
        string programLog = GL.GetProgramInfoLog(shader.Handle);
        if (linkStatus == 0) Logger.Error(programLog);
        else Logger.Verbose(programLog);
        Logger.Verbose($"Shader program status: {linkStatus}");

        return shader;
    }
}