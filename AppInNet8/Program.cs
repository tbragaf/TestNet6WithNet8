// See https://aka.ms/new-console-template for more information

using System.Reflection;

void PrintFiles(IEnumerable<string> files) {
    foreach (var file in files) {
        if (!file.EndsWith(".dll")) {
            continue;
        }

        try {
            var assembly = Assembly.LoadFrom(file);
            var constructorArguments = assembly.CustomAttributes.SelectMany(c => c.ConstructorArguments);
            var netArguments = constructorArguments.Where(a => a.ToString().Contains(".NET") && a.ToString().Contains("Version="));
    
            foreach (var arg in netArguments) {
                Console.WriteLine(file + "; " + arg.Value + ";");
            }
        } catch (BadImageFormatException) {
            // Not a .NET assembly, continue to the next file
        } catch (FileLoadException e) {
            if (e.Message.Contains("Assembly with same name is already loaded")) {
                continue;
            }
            
            if (e.Message.Contains("Could not load file or assembly")) {
                Console.WriteLine($"Error processing '{file}': {e.Message}");    
            }
        } catch (Exception e) {
            Console.WriteLine($"Error processing '{file}': {e.Message}");
        }
    }
}

IEnumerable<string> GetAllFiles(string path) {
    var queue = new Queue<string>();
    queue.Enqueue(path);
    
    while (queue.Count > 0) {
        path = queue.Dequeue();
        try {
            foreach (var subDir in Directory.GetDirectories(path)) {
                queue.Enqueue(subDir);
            }
        } catch(Exception ex) {
            Console.Error.WriteLine(ex);
        }
        
        string[]? files = null;
        
        try {
            files = Directory.GetFiles(path);
        } catch (Exception ex) {
            Console.Error.WriteLine(ex);
        }
        
        if (files != null) {
            for (var i = 0 ; i < files.Length ; i++) {
                yield return files[i];
            }
        }
    }
}

var path = Environment.CurrentDirectory;
PrintFiles(GetAllFiles(path));