require 'tempfile'

require 'dndn_sexpressions'
require 'dndn'

include Dndn

if ARGV.length != 1
  $stderr.puts "Usage: ruby dndn_tachy.rb FILE"
end

fn = ARGV.first
data = File.read(fn)
tf = Tempfile.new(File.basename(fn))
sexps = dndn(DndnSExpressionParser.parse(File.read(fn)))
src = sexps.map{|sexp| sexp.to_sexp}.join("\n")
puts src
tf.print(src)
tf.close
if RUBY_PLATFORM =~ /mswin/
  system "TachyRepl.exe #{tf.path}"
else
  system "mono TachyRepl.exe #{tf.path}"
end
